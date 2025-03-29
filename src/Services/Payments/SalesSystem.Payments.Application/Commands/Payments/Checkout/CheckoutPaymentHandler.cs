using MediatR;
using SalesSystem.Payments.Application.Services;
using SalesSystem.Payments.Domain.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Payments.Application.Commands.Payments.Checkout
{
    public sealed class CheckoutPaymentHandler(IPaymentFacade paymentFacade,
                                               INotificator notificator,
                                               IPaymentRepository paymentRepository)
                                             : IRequestHandler<CheckoutPaymentCommand, Response<CheckoutPaymentResponse>>
    {
        public async Task<Response<CheckoutPaymentResponse>> Handle(CheckoutPaymentCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<CheckoutPaymentResponse>.Failure(request.GetErrorMessages());

            var result = await paymentFacade.MakeCheckoutAsync(request).ConfigureAwait(false);

            if (!result.IsSuccess || result.Data is null)
            {
                notificator.HandleNotification(new("Something failed during checkout."));
                return Response<CheckoutPaymentResponse>.Failure(notificator.GetNotifications());
            }

            paymentRepository.Create(new(request.OrderId, request.CustomerId, request.Value));

            if (!await paymentRepository.UnitOfWork.CommitAsync())
            {
                notificator.HandleNotification(new("Fail to persist payment data."));
                return Response<CheckoutPaymentResponse>.Failure(notificator.GetNotifications());
            }

            return Response<CheckoutPaymentResponse>.Success(new(result.Data.SessionId, result.Data.OrderCode), code: 204);
        }
    }
}