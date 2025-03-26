using SalesSystem.Payments.Application.Commands.Payments.Checkout;
using SalesSystem.Payments.Application.Commands.Payments.Confirm;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Payments.Application.Services
{
    public interface IPaymentFacade
    {
        Task<Response<CheckoutPaymentResponse>> MakeCheckoutAsync(CheckoutPaymentCommand command);
        Task<Response<ConfirmPaymentResponse>> ConfirmPaymentAsync(string webhookSecret);
    }
}
