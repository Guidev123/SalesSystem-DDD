using SalesSystem.Payments.Application.Facades;
using SalesSystem.Payments.Application.Mappers;
using SalesSystem.Payments.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Payments.Application.Commands.Payments.Checkout
{
    public sealed class CheckoutPaymentHandler(IPaymentFacade paymentFacade,
                                               INotificator notificator,
                                               IPaymentRepository paymentRepository)
                                             : CommandHandler<CheckoutPaymentCommand, CheckoutPaymentResponse>(notificator)
    {
        public override async Task<Response<CheckoutPaymentResponse>> ExecuteAsync(CheckoutPaymentCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new CheckoutPaymentValidation(), request))
                return Response<CheckoutPaymentResponse>.Failure(GetNotifications());

            var result = await paymentFacade.MakeCheckoutAsync(request).ConfigureAwait(false);

            if (!result.IsSuccess || result.Data is null)
            {
                Notify("Something failed during checkout.");
                return Response<CheckoutPaymentResponse>.Failure(GetNotifications());
            }

            paymentRepository.Create(request.MapToPayment());

            if (!await paymentRepository.UnitOfWork.CommitAsync())
            {
                Notify("Fail to persist payment data.");
                return Response<CheckoutPaymentResponse>.Failure(GetNotifications());
            }

            return Response<CheckoutPaymentResponse>.Success(new(result.Data.SessionId, result.Data.OrderCode));
        }
    }
}