using SalesSystem.Sales.Application.DTOs;
using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Sales.Application.Queries.Orders.GetCustomerOrders
{
    public record GetCustomerOrdersQuery(int pageNumber, int pageSize, Guid CustomerId) : IPagedQuery<IEnumerable<OrderDto>>;
}