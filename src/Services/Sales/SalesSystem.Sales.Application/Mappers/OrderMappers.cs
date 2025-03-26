using SalesSystem.Sales.Application.Commands.Orders.AddOrderItem;
using SalesSystem.Sales.Application.DTOs;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.SharedKernel.Communication.DTOs;

namespace SalesSystem.Sales.Application.Mappers
{
    public static class OrderMappers
    {
        public static OrderItem MapOrderItemToEntity(this AddOrderItemCommand command)
            => new(command.ProductId, command.Name, command.Quantity, command.UnitPrice);

        public static CartDTO MapOrderToCartDTO(this Order order, IEnumerable<CartItemDTO> items, string? voucherCode = null)
            => new(order.Id, order.Discount + order.Price, order.Price, order.Discount, voucherCode, items.ToList());

        public static CartItemDTO MapOrderItemToCartItemDTO(this OrderItem orderItem)
            => new(orderItem.ProductId, orderItem.ProductName, orderItem.Quantity, orderItem.UnitPrice, orderItem.CalculatePrice());

        public static OrderDTO MapFromEntity(this Order order)
            => new(order.Code, order.Price, order.CreatedAt, nameof(order.Status));

        public static OrderProductsListDTO MapFromEntityToOrderProductsListDTO(List<OrderItem> items, Guid orderId)
        {
            var listItems = new List<ItemDTO>();

            items.ForEach(x => listItems.Add(new ItemDTO(x.Id, x.Quantity)));
            return new OrderProductsListDTO(orderId, listItems);
        }
    }
}