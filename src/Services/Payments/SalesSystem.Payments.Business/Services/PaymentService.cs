using SalesSystem.Payments.Business.Enums;
using SalesSystem.Payments.Business.Interfaces;
using SalesSystem.Payments.Business.Models;
using SalesSystem.SharedKernel.Communication.DTOs;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Payments.Business.Services
{
    public sealed class PaymentService(
            IPaymentRepository paymentRepository,
            IPaymentFacade paymentFacade,
            INotificator notificator
            ) : IPaymentService
    {
        public async Task<Response<Transaction>> CheckoutOrderAsync(PaymentOrderDTO paymentOrder)
        {
            var order = CreateOrder(paymentOrder);
            var payment = CreatePayment(paymentOrder);

            var transactionResponse = await ProcessPaymentAsync(order, payment);
            if (!IsPaymentSuccessful(transactionResponse) || transactionResponse.Data is null)
                return HandlePaymentFailure();

            return await HandleTransactionStatusAsync(transactionResponse.Data);
        }

        private Order CreateOrder(PaymentOrderDTO paymentOrder)
            => new(
                paymentOrder.Total,
                paymentOrder.CustomerEmail,
                paymentOrder.OrderCode,
                paymentOrder.CustomerId
            );

        private Payment CreatePayment(PaymentOrderDTO paymentOrder)
            => new(paymentOrder.OrderId, paymentOrder.Total);

        private async Task<Response<Transaction>> ProcessPaymentAsync(Order order, Payment payment)
            => await paymentFacade.MakePaymentAsync(order).ConfigureAwait(false);

        private bool IsPaymentSuccessful(Response<Transaction> transactionResponse)
            => transactionResponse.IsSuccess && transactionResponse.Data is not null;

        private Response<Transaction> HandlePaymentFailure()
        {
            notificator.HandleNotification(new("Fail to make payment."));
            return Response<Transaction>.Failure(notificator.GetNotifications());
        }

        private async Task<Response<Transaction>> HandleTransactionStatusAsync(Transaction transaction)
            => transaction.Status == ETransactionStatus.CheckoutPayment
                ? await ProcessCheckoutPaymentAsync(transaction)
                : HandleInvalidStatus();

        private async Task<Response<Transaction>> ProcessCheckoutPaymentAsync(Transaction transaction)
        {
            var payment = CreatePaymentForPersistence(transaction);
            PersistPaymentData(payment, transaction);

            var isPersisted = await CommitTransactionAsync();
            return isPersisted
                ? Response<Transaction>.Success(transaction)
                : HandlePersistenceFailure();
        }

        private Payment CreatePaymentForPersistence(Transaction transaction)
            => new(transaction.OrderId, transaction.Total);

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
    }
}
