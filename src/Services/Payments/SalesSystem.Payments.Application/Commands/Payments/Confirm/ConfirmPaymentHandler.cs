using SalesSystem.Payments.Application.Facades;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Payments.Application.Commands.Payments.Confirm
{
    public sealed class ConfirmPaymentHandler(IPaymentFacade paymentFacade,
                                              INotificator notificator)
                                            : CommandHandler<ConfirmPaymentCommand, ConfirmPaymentResponse>(notificator)
    {
        public override async Task<Response<ConfirmPaymentResponse>> ExecuteAsync(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new ConfirmPaymentValidation(), request))
                return Response<ConfirmPaymentResponse>.Failure(GetNotifications());

            return await paymentFacade.ConfirmPaymentAsync(request.WebhookSecret).ConfigureAwait(false);
        }
    }
}