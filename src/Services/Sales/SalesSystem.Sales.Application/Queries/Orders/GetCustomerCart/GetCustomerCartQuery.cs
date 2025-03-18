using SalesSystem.Sales.Application.DTOs;
using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Sales.Application.Queries.Orders.GetCustomerCart
{
    public record GetCustomerCartQuery(Guid CustomerId) : IQuery<CartDTO>;
}
