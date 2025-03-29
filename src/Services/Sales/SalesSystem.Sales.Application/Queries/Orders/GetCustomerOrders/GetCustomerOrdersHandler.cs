using MediatR;
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
                                               : IRequestHandler<GetCustomerOrdersQuery, PagedResponse<IEnumerable<OrderDTO>>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly INotificator _notificator = notificator;

        public async Task<PagedResponse<IEnumerable<OrderDTO>>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllByCutomerIdAsync(request.pageSize, request.pageNumber, request.CustomerId).ConfigureAwait(false);

            orders = orders.Where(x => x.Status == EOrderStatus.Paid || x.Status == EOrderStatus.Canceled);

            if (!orders.Any())
            {
                _notificator.HandleNotification(new("Orders not found."));
                return PagedResponse<IEnumerable<OrderDTO>>.Failure(_notificator.GetNotifications(), code: 404);
            }

            return PagedResponse<IEnumerable<OrderDTO>>.Success(orders.Select(x => x.MapFromEntity()), orders.Count(),
                                                                request.pageNumber, request.pageSize);
        }
    }
}