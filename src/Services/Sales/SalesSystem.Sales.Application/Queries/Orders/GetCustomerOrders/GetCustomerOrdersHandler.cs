using MidR.Interfaces;
using SalesSystem.Sales.Application.DTOs;
using SalesSystem.Sales.Application.Mappers;
using SalesSystem.Sales.Domain.Enums;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Queries.Orders.GetCustomerOrders
{
    public sealed class GetCustomerOrdersHandler(IOrderRepository orderRepository,
                                                 INotificator notificator)
                                               : IRequestHandler<GetCustomerOrdersQuery, PagedResponse<IEnumerable<OrderDto>>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly INotificator _notificator = notificator;

        public async Task<PagedResponse<IEnumerable<OrderDto>>> ExecuteAsync(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllByCutomerIdAsync(request.CustomerId).ConfigureAwait(false);

            var pagedOrders = orders.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            orders = pagedOrders.Where(x => x.Status == EOrderStatus.Paid || x.Status == EOrderStatus.Canceled);

            if (!orders.Any())
            {
                _notificator.HandleNotification(new("Orders not found."));
                return PagedResponse<IEnumerable<OrderDto>>.Failure(_notificator.GetNotifications(), code: 404);
            }

            return PagedResponse<IEnumerable<OrderDto>>.Success(orders.Select(x => x.MapFromEntity()), orders.Count(),
                                                                request.PageNumber, request.PageSize);
        }
    }
}