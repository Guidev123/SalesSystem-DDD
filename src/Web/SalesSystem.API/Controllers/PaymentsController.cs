using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesSystem.Payments.ACL.Configurations;
using SalesSystem.Payments.Application.Commands.Payments.Checkout;
using SalesSystem.Payments.Application.Commands.Payments.Confirm;
using SalesSystem.SharedKernel.Communication.Mediator;

namespace SalesSystem.API.Controllers
{
    [Route("api/v1/payments")]
    public class PaymentsController(IMediatorHandler mediatorHandler,
                                    IOptions<StripeSettings> stripeSettings,
                                    IHttpContextAccessor httpContextAccessor)
                                  : MainController(httpContextAccessor)
    {
        [HttpPost("checkout")]
        public async Task<IResult> CheckoutAsync(CheckoutPaymentCommand command)
        {
            command.SetCustomerCredentials(GetUserId(), GetUserEmail());
            return CustomResponse(await mediatorHandler.SendCommand(command));
        }

        [HttpPost("confirm")]
        public async Task<IResult> ConfirmPaymentWebhookAsync()
            => CustomResponse(await mediatorHandler.SendCommand(new ConfirmPaymentCommand(stripeSettings.Value.WebhookSecret)));
    }
}
