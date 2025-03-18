using MediatR;
using SalesSystem.Sales.Application.Events;
using SalesSystem.Sales.Application.Mappers;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Commands.Orders.AddOrderItem
{
    public sealed class AddOrderItemHandler(INotificator notificator,
                                            IOrderRepository orderRepository)
                                          : IRequestHandler<AddOrderItemCommand, Response<Guid>>
    {
        private readonly INotificator _notificator = notificator;
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<Response<Guid>> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<Guid>.Failure(request.GetErrorMessages());

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(request.CustomerId);
            var orderItem = request.MapOrderItemToEntity();

            var response = order is null
                ? HandleDraftOrder(request.CustomerId, orderItem, request.UnitPrice, request.Quantity)
                : HandleExistentOrder(order, orderItem, request.UnitPrice, request.Quantity);

            return response.IsSuccess
                ? await PersistDataAsync(orderItem.Id)
                : Response<Guid>.Failure(_notificator.GetNotifications());
        }

        private Response<Order> HandleExistentOrder(Order order, OrderItem orderItem, decimal unitPrice, int quantity)
        {
            var existentOrder = GetExistentOrderItem(order, orderItem.Id);
            if (existentOrder is null)
            {
                _notificator.HandleNotification(new("Fail to get order item."));
                return Response<Order>.Failure(_notificator.GetNotifications());
            }

            order.AddItem(orderItem);

            if (order.ItemAlreadyExists(orderItem))
                _orderRepository.UpdateItem(existentOrder);
            else
                _orderRepository.AddOrderItem(orderItem);

            order.AddEvent(new OrderUpdatedEvent(order.Id, order.CustomerId, order.Price));
            CreateOrderItemAddedEvent(order, orderItem.ProductId, unitPrice, quantity, orderItem.ProductName);

            return Response<Order>.Success(order);
        }

        private Response<Order> HandleDraftOrder(Guid customerId, OrderItem orderItem, decimal unitPrice, int quantity)
        {
            var order = Order.OrderFactory.NewDraftOrder(customerId);
            order.AddItem(orderItem);

            _orderRepository.Create(order);

            order.AddEvent(new DraftOrderStartedEvent(customerId, order.Id));
            CreateOrderItemAddedEvent(order, orderItem.ProductId, unitPrice, quantity, orderItem.ProductName);

            return Response<Order>.Success(order);
        }

        private async Task<Response<Guid>> PersistDataAsync(Guid orderItemId)
        {
            if (await _orderRepository.UnitOfWork.CommitAsync())
            {
                _notificator.HandleNotification(new("Fail to persist data."));
                return Response<Guid>.Failure(_notificator.GetNotifications());
            }

            return Response<Guid>.Success(orderItemId);
        }

        private static OrderItem? GetExistentOrderItem(Order order, Guid orderItemId)
            => order.OrderItems.FirstOrDefault(oi => oi.Id == orderItemId);

        private static void CreateOrderItemAddedEvent(Order order, Guid productId, decimal unitPrice, int quantity, string productName)
            => order.AddEvent(new OrderItemAddedEvent(order.Id, order.CustomerId, productId, unitPrice, quantity, productName));
    }
}
