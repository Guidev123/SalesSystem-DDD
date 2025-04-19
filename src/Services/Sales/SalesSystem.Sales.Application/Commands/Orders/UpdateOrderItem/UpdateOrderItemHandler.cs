using SalesSystem.Sales.Application.Events;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Commands.Orders.UpdateOrderItem
{
    public sealed class UpdateOrderItemHandler(IOrderRepository orderRepository,
                                               INotificator notificator)
                                             : CommandHandler<UpdateOrderItemCommand, UpdateOrderItemResponse>(notificator)
    {
        public override async Task<Response<UpdateOrderItemResponse>> ExecuteAsync(UpdateOrderItemCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new UpdateOrderItemValidation(), request))
                return Response<UpdateOrderItemResponse>.Failure(GetNotifications());

            var order = await orderRepository.GetDraftOrderByCustomerIdAsync(request.CustomerId);

            if (order is null)
            {
                Notify("Order not foud.");
                return Response<UpdateOrderItemResponse>.Failure(GetNotifications(), code: 404);
            }

            var orderItemResult = await GetOrderItemAsync(order, request.ProductId).ConfigureAwait(false);
            if (!orderItemResult.IsSuccess || orderItemResult.Data is null)
                return Response<UpdateOrderItemResponse>.Failure(GetNotifications(), code: 404);

            if (!UpdateUnitiesAsync(orderItemResult.Data, request, order))
                return Response<UpdateOrderItemResponse>.Failure(GetNotifications());

            return await PersistDataAsync(order, orderItemResult.Data, request.CustomerId).ConfigureAwait(false);
        }

        private bool UpdateUnitiesAsync(OrderItem orderItem, UpdateOrderItemCommand command, Order order)
        {
            if (orderItem.Quantity + command.Quantity > Order.MAX_ITEM_QUANTITY)
            {
                Notify($"The maximum quantity of items in the order is {Order.MAX_ITEM_QUANTITY}.");
                return false;
            }

            order.UpdateUnities(orderItem, command.Quantity);
            return true;
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

        private async Task<Response<UpdateOrderItemResponse>> PersistDataAsync(Order order, OrderItem item, Guid customerId)
        {
            orderRepository.UpdateItem(item);
            orderRepository.Update(order);

            if (!await orderRepository.UnitOfWork.CommitAsync().ConfigureAwait(false))
            {
                Notify("Fail to persist data.");
                return Response<UpdateOrderItemResponse>.Failure(GetNotifications());
            }

            order.AddEvent(new UpdatedOrderItemEvent(order.Id, customerId, order.Price, item.Quantity));

            return Response<UpdateOrderItemResponse>.Success(default, code: 204);
        }
    }
}