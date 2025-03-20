using SalesSystem.Sales.Application.DTOs;
using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Sales.Application.Queries.Orders.GetCustomerOrders
{
    public record GetCustomerOrdersQuery(int pageNumber, int pageSize, Guid CustomerId) : IPagedQuery<IEnumerable<OrderDTO>>;
}