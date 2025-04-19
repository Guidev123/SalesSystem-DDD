using SalesSystem.Sales.Application.Events;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Commands.Orders.RemoveOrderItem
{
    public sealed class RemoveOrderItemHandler(IOrderRepository orderRepository,
                                               INotificator notificator)
                                             : CommandHandler<RemoveOrderItemCommand, RemoveOrderItemResponse>(notificator)
    {
        public override async Task<Response<RemoveOrderItemResponse>> ExecuteAsync(RemoveOrderItemCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new RemoveOrderItemValidation(), request))
                return Response<RemoveOrderItemResponse>.Failure(GetNotifications());

            var order = await orderRepository.GetDraftOrderByCustomerIdAsync(request.CustomerId);

            if (order is null)
            {
                Notify("Order not foud.");
                return Response<RemoveOrderItemResponse>.Failure(GetNotifications(), code: 404);
            }

            var orderItemResult = await GetOrderItemAsync(order, request.ProductId).ConfigureAwait(false);
            if (!orderItemResult.IsSuccess || orderItemResult.Data is null)
                return Response<RemoveOrderItemResponse>.Failure(GetNotifications(), code: 404);

            order.RemoveItem(orderItemResult.Data);

            return await PersistDataAsync(order, orderItemResult.Data, request).ConfigureAwait(false);
        }

        private async Task<Response<OrderItem>> GetOrderItemAsync(Order order, Guid productId)
        {
            var orderItem = await orderRepository.GetItemByOrderIdAsync(order.Id, productId);
            if (orderItem is not null && !order.ItemAlreadyExists(orderItem))
            {
                Notify("Order item not foud.");
                return Response<OrderItem>.Failure(GetNotifications(), code: 404);
            }

            return Response<OrderItem>.Success(orderItem);
        }

        private async Task<Response<RemoveOrderItemResponse>> PersistDataAsync(Order order, OrderItem item, RemoveOrderItemCommand request)
        {
            orderRepository.RemoveItem(item);
            orderRepository.Update(order);

            if (!await orderRepository.UnitOfWork.CommitAsync().ConfigureAwait(false))
            {
                Notify("Fail to persist data.");
                return Response<RemoveOrderItemResponse>.Failure(GetNotifications());
            }

            order.AddEvent(new RemovedOrderItemEvent(order.Id, request.CustomerId, item.ProductId));
            order.AddEvent(new UpdatedOrderItemEvent(order.Id, request.CustomerId, order.Price, item.Quantity));

            return Response<RemoveOrderItemResponse>.Success(default, code: 204);
        }
    }
}