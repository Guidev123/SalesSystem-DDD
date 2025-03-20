using MediatR;
using SalesSystem.Sales.Application.Events;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Commands.Orders.RemoveOrderItem
{
    public sealed class RemoveOrderItemHandler(IOrderRepository orderRepository,
                                               INotificator notificator)
                                             : IRequestHandler<RemoveOrderItemCommand, Response<RemoveOrderItemResponse>>
    {
        private readonly INotificator _notificator = notificator;
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<Response<RemoveOrderItemResponse>> Handle(RemoveOrderItemCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<RemoveOrderItemResponse>.Failure(request.GetErrorMessages());

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(request.CustomerId);

            if (order is null)
            {
                _notificator.HandleNotification(new("Order not foud."));
                return Response<RemoveOrderItemResponse>.Failure(_notificator.GetNotifications(), code: 404);
            }

            var orderItemResult = await GetOrderItemAsync(order, request.ProductId).ConfigureAwait(false);
            if (!orderItemResult.IsSuccess || orderItemResult.Data is null)
                return Response<RemoveOrderItemResponse>.Failure(_notificator.GetNotifications(), code: 404);

            order.RemoveItem(orderItemResult.Data);

            return await PersistDataAsync(order, orderItemResult.Data, request).ConfigureAwait(false);
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

        private async Task<Response<RemoveOrderItemResponse>> PersistDataAsync(Order order, OrderItem item, RemoveOrderItemCommand request)
        {
            _orderRepository.RemoveItem(item);
            _orderRepository.Update(order);

            if (!await _orderRepository.UnitOfWork.CommitAsync().ConfigureAwait(false))
            {
                _notificator.HandleNotification(new("Fail to persist data."));
                return Response<RemoveOrderItemResponse>.Failure(_notificator.GetNotifications());
            }

            order.AddEvent(new RemovedOrderItemEvent(order.Id, request.CustomerId, item.ProductId));
            order.AddEvent(new UpdatedOrderItemEvent(order.Id, request.CustomerId, order.Price, item.Quantity));

            return Response<RemoveOrderItemResponse>.Success(default, code: 204);
        }
    }
}