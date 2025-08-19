using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Events.IntegrationEvents.Orders;
using SalesSystem.SharedKernel.Models;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Commands.Orders.CancelProcessingReverseStock
{
    public sealed class CancelOrderProcessingReverseStockHandler(IOrderRepository orderRepository,
                                                                 INotificator notificator)
                                                               : CommandHandler<CancelOrderProcessingReverseStockCommand, CancelOrderProcessingReverseStockResponse>(notificator)
    {
        public override async Task<Response<CancelOrderProcessingReverseStockResponse>> ExecuteAsync(CancelOrderProcessingReverseStockCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new CancelOrderProcessingReverseStockValidation(), request))
                return Response<CancelOrderProcessingReverseStockResponse>.Failure(GetNotifications());

            var order = await orderRepository.GetByIdAsync(request.OrderId).ConfigureAwait(false);
            if (order is null)
            {
                Notify("Order not found.");
                return Response<CancelOrderProcessingReverseStockResponse>.Failure(GetNotifications(), code: 404);
            }

            var listItems = new List<Item>();
            foreach (var item in order.OrderItems)
            {
                listItems.Add(new Item(item.ProductId, item.Quantity));
            }

            order.AddEvent(new OrderProcessingCanceledIntegrationEvent(order.Id, order.CustomerId, new OrderProductsList(order.Id, listItems)));
            order.DraftOrder();

            orderRepository.Update(order);
            return await PersistDataAsync();
        }

        private async Task<Response<CancelOrderProcessingReverseStockResponse>> PersistDataAsync()
        {
            if (!await orderRepository.UnitOfWork.CommitAsync())
            {
                Notify("Fail to persist data.");
                return Response<CancelOrderProcessingReverseStockResponse>.Failure(GetNotifications());
            }

            return Response<CancelOrderProcessingReverseStockResponse>.Success(default);
        }
    }
}