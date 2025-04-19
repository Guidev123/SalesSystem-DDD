using SalesSystem.Sales.Application.DTOs;
using SalesSystem.Sales.Application.Mappers;
using SalesSystem.Sales.Domain.Enums;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Queries.Orders.GetCustomerOrders
{
    public sealed class GetCustomerOrdersHandler(IOrderRepository orderRepository,
                                                 INotificator notificator)
                                               : PagedQueryHandler<GetCustomerOrdersQuery, IEnumerable<OrderDto>>(notificator)
    {
        public override async Task<PagedResponse<IEnumerable<OrderDto>>> ExecuteAsync(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.GetAllByCutomerIdAsync(request.CustomerId).ConfigureAwait(false);

            var pagedOrders = orders.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            orders = pagedOrders.Where(x => x.Status == EOrderStatus.Paid || x.Status == EOrderStatus.Canceled);

            if (!orders.Any())
            {
                Notify("Orders not found.");
                return PagedResponse<IEnumerable<OrderDto>>.Failure(GetNotifications(), code: 404);
            }

            return PagedResponse<IEnumerable<OrderDto>>.Success(orders.Select(x => x.MapFromEntity()), orders.Count(),
                                                                request.PageNumber, request.PageSize);
        }
    }
}