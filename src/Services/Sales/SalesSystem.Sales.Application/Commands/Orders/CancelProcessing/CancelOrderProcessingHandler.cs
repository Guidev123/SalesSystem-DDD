using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Commands.Orders.CancelProcessing
{
    public sealed class CancelOrderProcessingHandler(IOrderRepository orderRepository,
                                                                 INotificator notificator)
                                                               : CommandHandler<CancelOrderProcessingCommand, CancelOrderProcessingResponse>(notificator)
    {
        public override async Task<Response<CancelOrderProcessingResponse>> ExecuteAsync(CancelOrderProcessingCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new CancelOrderProcessingValidation(), request))
                return Response<CancelOrderProcessingResponse>.Failure(GetNotifications());

            var order = await orderRepository.GetByIdAsync(request.OrderId).ConfigureAwait(false);
            if (order is null)
            {
                Notify("Order not found.");
                return Response<CancelOrderProcessingResponse>.Failure(GetNotifications(), code: 404);
            }

            order.DraftOrder();
            orderRepository.Update(order);

            return await PersistDataAsync();
        }

        private async Task<Response<CancelOrderProcessingResponse>> PersistDataAsync()
        {
            if (!await orderRepository.UnitOfWork.CommitAsync())
            {
                Notify("Fail to persist data.");
                return Response<CancelOrderProcessingResponse>.Failure(GetNotifications());
            }

            return Response<CancelOrderProcessingResponse>.Success(default);
        }
    }
}