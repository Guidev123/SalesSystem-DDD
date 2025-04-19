using SalesSystem.Sales.Application.DTOs;
using SalesSystem.Sales.Application.Mappers;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Queries.Orders.GetCustomerCart
{
    public sealed class GetCustomerCartHandler(IOrderRepository orderRepository,
                                               INotificator notificator)
                                             : QueryHandler<GetCustomerCartQuery, CartDto>(notificator)
    {
        public override async Task<Response<CartDto>> ExecuteAsync(GetCustomerCartQuery request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetDraftOrderByCustomerIdAsync(request.CustomerId).ConfigureAwait(false);
            if (order is null)
            {
                Notify("Cart not found.");
                return Response<CartDto>.Failure(GetNotifications(), code: 404);
            }

            var cartItem = order.OrderItems.Select(x => x.MapOrderItemToCartItemDTO());
            var cart = order.Voucher is not null
                ? order.MapOrderToCartDTO(cartItem, order.Voucher.Code)
                : order.MapOrderToCartDTO(cartItem);

            return Response<CartDto>.Success(cart);
        }
    }
}