using SalesSystem.Sales.Application.DTOs;
using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Sales.Application.Queries.Orders.GetCustomerOrders
{
    public record GetCustomerOrdersQuery(int PageNumber, int PageSize, Guid CustomerId) : IPagedQuery<IEnumerable<OrderDto>>;
}