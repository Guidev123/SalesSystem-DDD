using SalesSystem.Sales.Application.Commands.Orders.AddOrderItem;
using SalesSystem.Sales.Application.DTOs;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.SharedKernel.DTOs;

namespace SalesSystem.Sales.Application.Mappers
{
    public static class OrderMappers
    {
        public static OrderItem MapOrderItemToEntity(this AddOrderItemCommand command)
            => new(command.ProductId, command.Name, command.Quantity, command.UnitPrice);

        public static CartDto MapOrderToCartDTO(this Order order, IEnumerable<CartItemDto> items, string? voucherCode = null)
            => new(order.Discount + order.Price, order.Price, order.Discount, voucherCode, items.ToList());

        public static CartItemDto MapOrderItemToCartItemDTO(this OrderItem orderItem)
            => new(orderItem.ProductId, orderItem.ProductName, orderItem.Quantity, orderItem.UnitPrice, orderItem.CalculatePrice());

        public static OrderDto MapFromEntity(this Order order)
            => new(order.Code, order.Price, order.CreatedAt, Enum.GetName(order.Status) ?? string.Empty);

        public static OrderProductsListDto MapFromEntityToOrderProductsListDTO(List<OrderItem> items, Guid orderId)
        {
            var listItems = new List<ItemDto>();

            items.ForEach(x => listItems.Add(new ItemDto(x.ProductId, x.Quantity)));
            return new OrderProductsListDto(orderId, listItems);
        }
    }
}