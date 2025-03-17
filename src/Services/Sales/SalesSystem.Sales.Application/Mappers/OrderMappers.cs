using SalesSystem.Sales.Application.Commands.AddOrderItem;
using SalesSystem.Sales.Domain.Entities;

namespace SalesSystem.Sales.Application.Mappers
{
    public static class OrderMappers
    {
        public static OrderItem MapOrderItemToEntity(this AddOrderItemCommand command)
            => new(command.ProductId, command.Name, command.Quantity, command.UnitPrice);
    }
}
