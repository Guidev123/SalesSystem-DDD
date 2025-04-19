using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Events.IntegrationEvents.Orders;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Commands.Orders.Finish
{
    public sealed class FinishOrderHandler(IOrderRepository orderRepository,
                                           INotificator notificator)
                                         : CommandHandler<FinishOrderCommand, FinishOrderResponse>(notificator)
    {
        public override async Task<Response<FinishOrderResponse>> ExecuteAsync(FinishOrderCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new FinishOrderValidation(), request))
                return Response<FinishOrderResponse>.Failure(GetNotifications());

            var order = await orderRepository.GetByIdAsync(request.OrderId).ConfigureAwait(false);
            if (order is null)
            {
                Notify("Order not found.");
                return Response<FinishOrderResponse>.Failure(GetNotifications(), code: 404);
            }

            order.PayOrder();
            orderRepository.Update(order);

            order.AddEvent(new FinishedOrderIntegrationEvent(request.OrderId));

            return await PersistDataAsync();
        }

        private async Task<Response<FinishOrderResponse>> PersistDataAsync()
        {
            if (await orderRepository.UnitOfWork.CommitAsync())
            {
                Notify("Fail to persist data.");
                return Response<FinishOrderResponse>.Failure(GetNotifications());
            }

            return Response<FinishOrderResponse>.Success(default);
        }
    }
}