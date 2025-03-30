using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
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
    public sealed class PaymentFacade(IStripeService stripeService,
                                      IHttpContextAccessor httpContext,
                                      IPaymentRepository paymentRepository,
                                      IOptions<StripeSettings> stripeSettings,
                                      INotificator notificator)
                                    : IPaymentFacade
    {
        public async Task<Response<ConfirmPaymentResponse>> ConfirmPaymentAsync(string webhookSecret)
        {
            var context = httpContext.HttpContext;
            if (context == null)
            {
                notificator.HandleNotification(new("HttpContext is null."));
                return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());
            }

            try
            {
                var json = await new StreamReader(context.Request.Body).ReadToEndAsync();
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    context.Request.Headers["Stripe-Signature"],
                    webhookSecret,
                    throwOnApiVersionMismatch: false
                );

                if (stripeEvent is null)
                {
                    notificator.HandleNotification(new("Invalid Stripe event."));
                    return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());
                }

                if (stripeEvent.Data.Object is not Charge charge)
                {
                    notificator.HandleNotification(new("Event data is not a Charge object."));
                    return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());
                }

                var response = await stripeService.ConfirmPaymentInternal(stripeEvent, charge);
                if (!response.IsSuccess) return response;

                var commitResult = await paymentRepository.UnitOfWork.CommitAsync();
                if (!commitResult)
                {
                    notificator.HandleNotification(new("Failed to persist transaction data."));
                    return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());
                }

                return Response<ConfirmPaymentResponse>.Success(default);
            }
            catch (StripeException ex)
            {
                notificator.HandleNotification(new($"Stripe error: {ex.Message}"));
                return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());
            }
            catch (Exception ex)
            {
                notificator.HandleNotification(new($"Unexpected error: {ex.Message}"));
                return Response<ConfirmPaymentResponse>.Failure(notificator.GetNotifications());
            }
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

        private StripeSettings GetStripeProperties() => stripeSettings.Value;
    }
}