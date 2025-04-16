using SalesSystem.Sales.Application.DTOs;
using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Sales.Application.Queries.Orders.GetCustomerCart
{
    public record GetCustomerCartQuery(Guid CustomerId) : IQuery<CartDto>;
}