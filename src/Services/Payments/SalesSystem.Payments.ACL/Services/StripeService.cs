using SalesSystem.Payments.ACL.Configurations;
using SalesSystem.Payments.ACL.Interfaces;
using SalesSystem.Payments.Application.Commands.Payments.Checkout;
using SalesSystem.Payments.Application.Commands.Payments.Confirm;
using SalesSystem.Payments.Domain.Entities;
using SalesSystem.SharedKernel.Messages.CommonMessages.IntegrationEvents.Payments;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;
using Stripe;
using Stripe.Checkout;

namespace SalesSystem.Payments.ACL.Services
{
    public sealed class StripeService(INotificator notificator) : IStripeService
    {
        public async Task<string?> CreateSessionAsync(CheckoutPaymentCommand command, StripeSettings stripeConfiguration)
        {
            var client = new StripeClient(stripeConfiguration.ApiKey);

            var options = new SessionCreateOptions
            {
                CustomerEmail = command.CustomerEmail,
                ClientReferenceId = command.CustomerId.ToString(),

                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Metadata = new Dictionary<string, string>
                {
                    { "order", command.OrderCode }
                }
                },
                PaymentMethodTypes = [stripeConfiguration.PaymentMethodTypes],
                LineItems = command.Products.Select(product => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "BRL",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Name,
                        },
                        UnitAmount = (int)Math.Round(product.Value * 100, 2),
                    },
                    Quantity = product.Quantity
                }).ToList(),
                Mode = stripeConfiguration.StripeMode,
                SuccessUrl = $"{stripeConfiguration.FrontendUrl}/orders/{command.OrderCode}/confirm",
                CancelUrl = $"{stripeConfiguration.FrontendUrl}/orders/{command.OrderCode}/cancel",
            };

            var service = new SessionService(client);
            var session = await service.CreateAsync(options);

            return session.Id;
        }

        public Response<ConfirmPaymentResponse> ConfirmPaymentInternal(Event stripeEvent, Charge charge, Payment payment)
        {
            try
            {
                if (stripeEvent.Type.Equals("charge.succeeded", StringComparison.OrdinalIgnoreCase)
                    && charge.Metadata.TryGetValue("order", out var orderNumber))
                {
                    var transaction = new Transaction(payment.OrderId, payment.Id, payment.Amount, orderNumber);

                    payment.AddTransaction(transaction);
                    payment.SetAsPaid(charge.Status, charge.Id);
                    payment.AddEvent(new PaymentSuccessfullyIntegrationEvent(payment.OrderId, payment.CustomerId));

                    return Response<ConfirmPaymentResponse>.Success(default);
                }

                payment.AddEvent(new PaymentFailedIntegrationEvent(payment.OrderId, payment.CustomerId));
                return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());
            }
            catch
            {
                payment.AddEvent(new PaymentFailedIntegrationEvent(payment.OrderId, payment.CustomerId));
                notificator.HandleNotification(new("Fail to verify payment."));
                return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());
            }
        }
    }
}