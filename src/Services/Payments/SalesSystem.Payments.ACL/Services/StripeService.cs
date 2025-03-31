using Microsoft.Extensions.Options;
using SalesSystem.Payments.ACL.Configurations;
using SalesSystem.Payments.ACL.Interfaces;
using SalesSystem.Payments.Application.Commands.Payments.Checkout;
using SalesSystem.Payments.Application.Commands.Payments.Confirm;
using SalesSystem.Payments.Domain.Entities;
using SalesSystem.Payments.Domain.Repositories;
using SalesSystem.SharedKernel.Messages.CommonMessages.IntegrationEvents.Payments;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;
using Stripe;
using Stripe.Checkout;

namespace SalesSystem.Payments.ACL.Services
{
    public sealed class StripeService(INotificator notificator,
                                      IPaymentRepository paymentRepository,
                                      IOptions<StripeSettings> stripeSettings) : IStripeService
    {
        private const string STRIPE_SUCCEEEDED_CHARGE_EVENT = "charge.succeeded";

        public async Task<string?> CreateSessionAsync(CheckoutPaymentCommand command, StripeSettings stripeConfiguration)
        {
            stripeConfiguration = stripeSettings.Value;

            var totalProductsValue = command.Products.Sum(product => product.Value * product.Quantity);
            var discount = totalProductsValue - command.Value;

            var client = new StripeClient(stripeConfiguration.ApiKey);

            var options = new SessionCreateOptions
            {
                CustomerEmail = command.CustomerEmail,
                ClientReferenceId = command.CustomerId.ToString(),

                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Metadata = new Dictionary<string, string>
                {
                    { "order", command.OrderCode },
                    { "customerId", command.CustomerId.ToString() }
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

            if (discount > 0)
            {
                var couponService = new CouponService(client);
                var couponOptions = new CouponCreateOptions
                {
                    AmountOff = (long)(discount * 100),
                    Currency = "BRL",
                    Duration = "once",
                    Name = "Voucher"
                };
                var coupon = await couponService.CreateAsync(couponOptions);

                options.Discounts =
                [
                    new SessionDiscountOptions { Coupon = coupon.Id }
                ];
            }

            var service = new SessionService(client);
            var session = await service.CreateAsync(options);

            return session.Id;
        }

        public async Task<Response<ConfirmPaymentResponse>> ConfirmPaymentInternal(Event stripeEvent, Charge charge)
        {
            if (!charge.Metadata.TryGetValue("customerId", out var customerId) || string.IsNullOrEmpty(customerId))
            {
                notificator.HandleNotification(new("Customer ID not found in metadata."));
                return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());
            }

            if (!Guid.TryParse(customerId, out var guidCustomerId))
            {
                notificator.HandleNotification(new("Invalid Customer ID format."));
                return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());
            }

            var payment = await paymentRepository.GetByCustomerIdAsync(guidCustomerId);
            if (payment is null)
            {
                notificator.HandleNotification(new("Payment not found for customer."));
                return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());
            }

            try
            {
                if (stripeEvent.Type.Equals(STRIPE_SUCCEEEDED_CHARGE_EVENT, StringComparison.OrdinalIgnoreCase))
                {
                    if (!charge.Metadata.TryGetValue("order", out var orderNumber) || string.IsNullOrEmpty(orderNumber))
                    {
                        notificator.HandleNotification(new("Order number not found in metadata."));
                        payment.SetAsFailed();
                        payment.AddEvent(new PaymentFailedIntegrationEvent(payment.OrderId, payment.CustomerId));
                        return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());
                    }

                    var transaction = new Transaction(payment.OrderId, payment.Id, payment.Amount, orderNumber);
                    payment.AddTransaction(transaction);
                    payment.SetAsPaid(charge.Status, charge.Id);

                    paymentRepository.Update(payment);
                    paymentRepository.CreateTransaction(transaction);

                    payment.AddEvent(new PaymentSuccessfullyIntegrationEvent(payment.OrderId, payment.CustomerId));

                    return Response<ConfirmPaymentResponse>.Success(default);
                }
                else
                {
                    payment.SetAsFailed();
                    payment.AddEvent(new PaymentFailedIntegrationEvent(payment.OrderId, payment.CustomerId));
                    notificator.HandleNotification(new($"Unhandled Stripe event type: {stripeEvent.Type}"));
                    return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());
                }
            }
            catch (Exception ex)
            {
                payment.SetAsFailed();
                payment.AddEvent(new PaymentFailedIntegrationEvent(payment.OrderId, payment.CustomerId));
                notificator.HandleNotification(new($"Payment processing error: {ex.Message}"));
                return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());
            }
        }
    }
}