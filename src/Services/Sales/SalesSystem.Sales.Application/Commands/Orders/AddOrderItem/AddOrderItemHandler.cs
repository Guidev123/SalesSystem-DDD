using SalesSystem.Sales.Application.Events;
using SalesSystem.Sales.Application.Mappers;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;
using static SalesSystem.Sales.Domain.Entities.Order;

namespace SalesSystem.Sales.Application.Commands.Orders.AddOrderItem
{
    public sealed class AddOrderItemHandler(INotificator notificator,
                                            IOrderRepository orderRepository)
                                          : CommandHandler<AddOrderItemCommand, AddOrderItemResponse>(notificator)
    {
        public override async Task<Response<AddOrderItemResponse>> ExecuteAsync(AddOrderItemCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new AddOrderItemValidation(), request))
                return Response<AddOrderItemResponse>.Failure(GetNotifications());

            var order = await orderRepository.GetDraftOrderByCustomerIdAsync(request.CustomerId);
            var orderItem = request.MapOrderItemToEntity();

            var response = order is null
                ? HandleDraftOrder(request.CustomerId, orderItem, request.UnitPrice, request.Quantity)
                : HandleExistentOrder(order, orderItem, request.UnitPrice, request.Quantity);

            return response.IsSuccess
                ? await PersistDataAsync(request.ProductId)
                : Response<AddOrderItemResponse>.Failure(GetNotifications());
        }

        private Response<Order> HandleExistentOrder(Order order, OrderItem orderItem, decimal unitPrice, int quantity)
        {
            var existentOrderItem = GetExistentOrderItem(order, orderItem.ProductId);
            var orderItemAlreadyExists = order.ItemAlreadyExists(orderItem);
            if (!ValidateOrderItemQuantity(order, orderItem, quantity))
                return Response<Order>.Failure(GetNotifications());

            order.AddItem(orderItem);

            if (orderItemAlreadyExists)
                orderRepository.UpdateItem(existentOrderItem!);
            else
                orderRepository.AddOrderItem(orderItem);

            orderRepository.Update(order);

            order.AddEvent(new UpdatedOrderItemEvent(order.Id, order.CustomerId, order.Price, quantity));
            CreateOrderItemAddedEvent(order, orderItem.ProductId, unitPrice, quantity, orderItem.ProductName);

            return Response<Order>.Success(order);
        }

        private Response<Order> HandleDraftOrder(Guid customerId, OrderItem orderItem, decimal unitPrice, int quantity)
        {
            var order = OrderFactory.NewDraftOrder(customerId);
            order.AddItem(orderItem);

            orderRepository.Create(order);

            order.AddEvent(new DraftOrderStartedEvent(customerId, order.Id));
            CreateOrderItemAddedEvent(order, orderItem.ProductId, unitPrice, quantity, orderItem.ProductName);

            return Response<Order>.Success(order);
        }

        private async Task<Response<AddOrderItemResponse>> PersistDataAsync(Guid productId)
        {
            if (!await orderRepository.UnitOfWork.CommitAsync())
            {
                Notify("Fail to persist data.");
                return Response<AddOrderItemResponse>.Failure(GetNotifications());
            }

            return Response<AddOrderItemResponse>.Success(new(productId));
        }

        private static OrderItem? GetExistentOrderItem(Order order, Guid orderItemId)
            => order.OrderItems.FirstOrDefault(oi => oi.ProductId == orderItemId);

        private static void CreateOrderItemAddedEvent(Order order, Guid productId, decimal unitPrice, int quantity, string productName)
            => order.AddEvent(new AddedOrderItemEvent(order.Id, order.CustomerId, productId, unitPrice, quantity, productName));

        private bool ValidateOrderItemQuantity(Order order, OrderItem item, int quantity)
        {
            var orderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (order.ItemAlreadyExists(item) && (orderItem?.Quantity + quantity) > MAX_ITEM_QUANTITY)
            {
                Notify($"The maximum quantity of items in the order is {MAX_ITEM_QUANTITY}.");
                return false;
            }

            return true;
        }
    }
}