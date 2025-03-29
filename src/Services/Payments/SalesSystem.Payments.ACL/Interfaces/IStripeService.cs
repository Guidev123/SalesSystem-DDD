using SalesSystem.Payments.ACL.Configurations;
using SalesSystem.Payments.Application.Commands.Payments.Checkout;
using SalesSystem.Payments.Application.Commands.Payments.Confirm;
using SalesSystem.Payments.Domain.Entities;
using SalesSystem.SharedKernel.Responses;
using Stripe;

namespace SalesSystem.Payments.ACL.Interfaces
{
    public interface IStripeService
    {
        Task<string?> CreateSessionAsync(CheckoutPaymentCommand command, StripeSettings stripeConfiguration);

        Response<ConfirmPaymentResponse> ConfirmPaymentInternal(Event stripeEvent, Charge charge, Payment payment);
    }
}