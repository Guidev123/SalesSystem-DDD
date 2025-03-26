using SalesSystem.Payments.ACL.Configurations;
using SalesSystem.Payments.ACL.Interfaces;
using SalesSystem.Payments.Business.Models;
using Stripe;
using Stripe.Checkout;

namespace SalesSystem.Payments.ACL.Services
{
    public sealed class StripeService : IStripeService
    {
        public async Task<string?> CreateSessionAsync(Order order, StripeSettings stripeConfiguration)
        {
            var client = new StripeClient(stripeConfiguration.ApiKey);

            var options = new SessionCreateOptions
            {
                CustomerEmail = order.CustomerEmail,
                ClientReferenceId = order.CustomerId.ToString(),

                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Metadata = new Dictionary<string, string>
                {
                    { "order", order.OrderCode }
                }
                },
                PaymentMethodTypes = [stripeConfiguration.PaymentMethodTypes],
                LineItems = order.Products.Select(product => new SessionLineItemOptions
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
                SuccessUrl = $"{stripeConfiguration.FrontendUrl}/orders/{order.OrderCode}/confirm",
                CancelUrl = $"{stripeConfiguration.FrontendUrl}/orders/{order.OrderCode}/cancel",
            };

            var service = new SessionService(client);
            var session = await service.CreateAsync(options);

            return session.Id;
        }
    }
}
