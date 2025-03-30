using SalesSystem.Payments.ACL.Configurations;
using SalesSystem.Payments.Application.Commands.Payments.Checkout;
using SalesSystem.Payments.Application.Commands.Payments.Confirm;
using SalesSystem.SharedKernel.Responses;
using Stripe;

namespace SalesSystem.Payments.ACL.Interfaces
{
    public interface IStripeService
    {
        Task<string?> CreateSessionAsync(CheckoutPaymentCommand command, StripeSettings stripeConfiguration);
        Task<Response<ConfirmPaymentResponse>> ConfirmPaymentInternal(Event stripeEvent, Charge charge);
    }
}