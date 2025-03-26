using Microsoft.AspNetCore.Http;
using SalesSystem.Payments.Business.Enums;
using SalesSystem.Payments.Business.Interfaces;
using SalesSystem.Payments.Business.Models;
using SalesSystem.SharedKernel.Communication.DTOs;
using SalesSystem.SharedKernel.Messages.CommonMessages.IntegrationEvents.Payments;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;
using Stripe;

namespace SalesSystem.Payments.Business.Services
{
    public sealed class PaymentService(
            IPaymentRepository paymentRepository,
            IHttpContextAccessor httpContextAccessor,
            IPaymentFacade paymentFacade,
            INotificator notificator
            ) : IPaymentService
    {
        public async Task<Response<Transaction>> CheckoutOrderAsync(PaymentOrderDTO paymentOrder, CancellationToken cancellationToken)
        {
            var order = CreateOrder(paymentOrder);

            var transactionResponse = await ProcessPaymentAsync(order);
            if (!IsPaymentSuccessful(transactionResponse) || transactionResponse.Data is null)
                return HandlePaymentFailure();

            return await HandleTransactionStatusAsync(transactionResponse.Data, paymentOrder.CustomerId);
        }

        public async Task ConfirmPaymentAsync(string webhookSecret, CancellationToken cancellationToken)
        {
            var context = httpContextAccessor.HttpContext;
            if (context is null) return;

            var stripeEvent = await GetStripeEventAsync(webhookSecret, cancellationToken);
            if (stripeEvent is null) return;

            var charge = stripeEvent.Data.Object as Charge ?? new Charge();
            var payment = await GetPaymentAsync(charge);
            if (!payment.IsSuccess || payment.Data is null) return;

            try
            {
                await ProcessPaymentAsync(stripeEvent, charge, payment.Data);
            }
            catch 
            {
                HandlePaymentFailure(payment.Data);
            }
        }

        #region Private Methods

        private async Task<Event> GetStripeEventAsync(string webhookSecret, CancellationToken cancellationToken)
        {
            var context = httpContextAccessor.HttpContext;
            var json = await new StreamReader(context.Request.Body).ReadToEndAsync(cancellationToken);
            return EventUtility.ConstructEvent(
                json,
                context.Request.Headers["Stripe-Signature"],
                webhookSecret,
                throwOnApiVersionMismatch: false
            );
        }

        private async Task<Response<Payment>> GetPaymentAsync(Charge charge)
        {
            if (string.IsNullOrEmpty(charge.CustomerId) || !Guid.TryParse(charge.CustomerId, out var customerId))
                return Response<Payment>.Failure([]);

            return Response<Payment>.Success(await paymentRepository.GetByCustomerIdAsync(customerId));
        }

        private async Task ProcessPaymentAsync(Event stripeEvent, Charge charge, Payment payment)
        {
            if (!stripeEvent.Type.Equals("charge.succeeded", StringComparison.OrdinalIgnoreCase) ||
                !charge.Metadata.TryGetValue("order", out var _))
            {
                HandlePaymentFailure(payment);
                return;
            }

            var transaction = new Transaction(payment.OrderId, payment.Id, payment.Amount);
            payment.AddTransaction(transaction);
            payment.SetAsPaid(charge.Status, charge.Id);

            paymentRepository.CreateTransaction(transaction);
            paymentRepository.Update(payment);

            if (await paymentRepository.UnitOfWork.CommitAsync())
            {
                payment.AddEvent(new PaymentSuccessfullyIntegrationEvent(payment.OrderId, payment.CustomerId));
            }
            else
            {
                payment.PurgeEvents();
                HandlePaymentFailure(payment);
            }
        }

        private void HandlePaymentFailure(Payment payment)
            => payment.AddEvent(new PaymentFailedIntegrationEvent(payment.OrderId, payment.CustomerId));

        private Order CreateOrder(PaymentOrderDTO paymentOrder)
            => new(
                paymentOrder.Total,
                paymentOrder.CustomerEmail,
                paymentOrder.OrderCode,
                paymentOrder.CustomerId
            );

        private async Task<Response<Transaction>> ProcessPaymentAsync(Order order)
            => await paymentFacade.MakePaymentAsync(order).ConfigureAwait(false);

        private bool IsPaymentSuccessful(Response<Transaction> transactionResponse)
            => transactionResponse.IsSuccess && transactionResponse.Data is not null;

        private Response<Transaction> HandlePaymentFailure()
        {
            notificator.HandleNotification(new("Fail to make payment."));
            return Response<Transaction>.Failure(notificator.GetNotifications());
        }

        private async Task<Response<Transaction>> HandleTransactionStatusAsync(Transaction transaction, Guid customerId)
            => transaction.Status == ETransactionStatus.CheckoutPayment
                ? await ProcessCheckoutPaymentAsync(transaction, customerId)
                : HandleInvalidStatus();

        private async Task<Response<Transaction>> ProcessCheckoutPaymentAsync(Transaction transaction, Guid customerId)
        {
            var payment = CreatePaymentForPersistence(transaction, customerId);
            PersistPaymentData(payment, transaction);

            var isPersisted = await CommitTransactionAsync();
            return isPersisted
                ? Response<Transaction>.Success(transaction)
                : HandlePersistenceFailure();
        }

        private Payment CreatePaymentForPersistence(Transaction transaction, Guid customerId)
            => new(transaction.OrderId, customerId, transaction.Total);

        private void PersistPaymentData(Payment payment, Transaction transaction)
        {
            paymentRepository.Create(payment);
            paymentRepository.CreateTransaction(transaction);
        }

        private async Task<bool> CommitTransactionAsync()
            => await paymentRepository.UnitOfWork.CommitAsync().ConfigureAwait(false);

        private Response<Transaction> HandlePersistenceFailure()
        {
            notificator.HandleNotification(new("Fail to persist data."));
            return Response<Transaction>.Failure(notificator.GetNotifications());
        }

        private Response<Transaction> HandleInvalidStatus()
        {
            notificator.HandleNotification(new("Fail to checkout order."));
            return Response<Transaction>.Failure(notificator.GetNotifications());
        }
        #endregion
    }
}
