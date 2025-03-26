using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SalesSystem.Payments.ACL.Configurations;
using SalesSystem.Payments.ACL.Interfaces;
using SalesSystem.Payments.Application.Commands.Payments.Checkout;
using SalesSystem.Payments.Application.Commands.Payments.Confirm;
using SalesSystem.Payments.Application.Services;
using SalesSystem.Payments.Domain.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;
using Stripe;

namespace SalesSystem.Payments.ACL.Services
{
    public sealed class PaymentFacade(IConfiguration configuration,
                                      IStripeService stripeService,
                                      IHttpContextAccessor httpContext,
                                      IPaymentRepository paymentRepository,
                                      INotificator notificator)
                                    : IPaymentFacade
    {
        public async Task<Response<ConfirmPaymentResponse>> ConfirmPaymentAsync(string webhookSecret)
        {
            var context = httpContext.HttpContext;

            if (context is null)
                return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());

            var json = await new StreamReader(context.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                context.Request.Headers["Stripe-Signature"],
                webhookSecret,
                throwOnApiVersionMismatch: false
            );

            if (stripeEvent.Data.Object is not Charge charge)
                return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());

            var payment = await paymentRepository.GetByCustomerIdAsync(Guid.Parse(charge.CustomerId));
            var response = stripeService.ConfirmPaymentInternal(stripeEvent, charge, payment);

            if (!response.IsSuccess) return response;

            return await paymentRepository.UnitOfWork.CommitAsync()
                ? Response<ConfirmPaymentResponse>.Success(default)
                : Response<ConfirmPaymentResponse>.Failure(["Fail to persist transaction data."]);
        }

        public async Task<Response<CheckoutPaymentResponse>> MakeCheckoutAsync(CheckoutPaymentCommand command)
        {
            var stripeResponse = await stripeService.CreateSessionAsync(command, GetStripeProperties())
                .ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(stripeResponse))
            {
                notificator.HandleNotification(new("Fail to create session with Stripe."));
                return Response<CheckoutPaymentResponse>.Failure(notificator.GetNotifications());
            }

            return Response<CheckoutPaymentResponse>.Success(new(stripeResponse, command.OrderCode));
        }

        private StripeSettings GetStripeProperties()
        {
            var apiKey = configuration["StripeSettings:ApiKey"] ?? string.Empty;
            var frontendUrl = configuration["StripeSettings:FrontendUrl"] ?? string.Empty;
            var stripeMode = configuration["StripeSettings:StripeMode"] ?? string.Empty;
            var paymentMethodTypes = configuration["StripeSettings:PaymentMethodTypes"] ?? string.Empty;

            return new(apiKey, frontendUrl, stripeMode, paymentMethodTypes);
        }
    }
}
