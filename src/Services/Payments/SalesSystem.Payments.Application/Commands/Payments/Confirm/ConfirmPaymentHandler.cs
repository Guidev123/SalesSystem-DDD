using MidR.Interfaces;
using SalesSystem.Payments.Application.Services;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Payments.Application.Commands.Payments.Confirm
{
    public sealed class ConfirmPaymentHandler(IPaymentFacade paymentFacade)
                                            : IRequestHandler<ConfirmPaymentCommand, Response<ConfirmPaymentResponse>>
    {
        public async Task<Response<ConfirmPaymentResponse>> ExecuteAsync(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<ConfirmPaymentResponse>.Failure(request.GetErrorMessages());

            return await paymentFacade.ConfirmPaymentAsync(request.WebhookSecret).ConfigureAwait(false);
        }
    }
}