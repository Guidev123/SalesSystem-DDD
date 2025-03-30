using MediatR;
using SalesSystem.Sales.Application.Events;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Commands.Orders.UpdateOrderItem
{
    public sealed class UpdateOrderItemHandler(IOrderRepository orderRepository,
                                               INotificator notificator)
                                             : IRequestHandler<UpdateOrderItemCommand, Response<UpdateOrderItemResponse>>
    {
        private readonly INotificator _notificator = notificator;
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<Response<UpdateOrderItemResponse>> Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<UpdateOrderItemResponse>.Failure(request.GetErrorMessages());

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(request.CustomerId);

            if (order is null)
            {
                _notificator.HandleNotification(new("Order not foud."));
                return Response<UpdateOrderItemResponse>.Failure(_notificator.GetNotifications(), code: 404);
            }

            var orderItemResult = await GetOrderItemAsync(order, request.ProductId).ConfigureAwait(false);
            if (!orderItemResult.IsSuccess || orderItemResult.Data is null)
                return Response<UpdateOrderItemResponse>.Failure(_notificator.GetNotifications(), code: 404);

            order.UpdateUnities(orderItemResult.Data, request.Quantity);

            return await PersistDataAsync(order, orderItemResult.Data, request.CustomerId).ConfigureAwait(false);
        }

        private async Task<Response<OrderItem>> GetOrderItemAsync(Order order, Guid productId)
        {
            var orderItem = await _orderRepository.GetItemByOrderIdAsync(order.Id, productId);
            if (orderItem is not null && !order.ItemAlreadyExists(orderItem))
            {
                _notificator.HandleNotification(new("Order item not foud."));
                return Response<OrderItem>.Failure(_notificator.GetNotifications(), code: 404);
            }

            return Response<OrderItem>.Success(orderItem);
        }

        private async Task<Response<UpdateOrderItemResponse>> PersistDataAsync(Order order, OrderItem item, Guid customerId)
        {
            _orderRepository.UpdateItem(item);
            _orderRepository.Update(order);

            if (!await _orderRepository.UnitOfWork.CommitAsync().ConfigureAwait(false))
            {
                _notificator.HandleNotification(new("Fail to persist data."));
                return Response<UpdateOrderItemResponse>.Failure(_notificator.GetNotifications());
            }

            order.AddEvent(new UpdatedOrderItemEvent(order.Id, customerId, order.Price, item.Quantity));

            return Response<UpdateOrderItemResponse>.Success(default, code: 204);
        }
    }
}