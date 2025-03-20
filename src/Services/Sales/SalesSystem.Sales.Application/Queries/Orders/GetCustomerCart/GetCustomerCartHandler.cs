using MediatR;
using SalesSystem.Sales.Application.DTOs;
using SalesSystem.Sales.Application.Mappers;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Queries.Orders.GetCustomerCart
{
    public sealed class GetCustomerCartHandler(IOrderRepository orderRepository,
                                               INotificator notificator)
                                             : IRequestHandler<GetCustomerCartQuery, Response<CartDTO>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly INotificator _notificator = notificator;

        public async Task<Response<CartDTO>> Handle(GetCustomerCartQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(request.CustomerId).ConfigureAwait(false);
            if (order is null)
            {
                _notificator.HandleNotification(new("Cart not found."));
                return Response<CartDTO>.Failure(_notificator.GetNotifications(), code: 404);
            }

            var cartItem = order.OrderItems.Select(x => x.MapOrderItemToCartItemDTO());
            var cart = order.Voucher is not null ? order.MapOrderToCartDTO(cartItem, order.Voucher.Code) : order.MapOrderToCartDTO(cartItem);

            return Response<CartDTO>.Success(cart);
        }
    }
}