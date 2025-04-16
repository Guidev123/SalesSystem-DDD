using MidR.Interfaces;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.DTOs;
using SalesSystem.SharedKernel.Events.IntegrationEvents.Orders;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Commands.Orders.CancelProcessingReverseStock
{
    public sealed class CancelOrderProcessingReverseStockHandler(IOrderRepository orderRepository,
                                                                 INotificator notificator)
                                                               : IRequestHandler<CancelOrderProcessingReverseStockCommand, Response<CancelOrderProcessingReverseStockResponse>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly INotificator _notificator = notificator;

        public async Task<Response<CancelOrderProcessingReverseStockResponse>> ExecuteAsync(CancelOrderProcessingReverseStockCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<CancelOrderProcessingReverseStockResponse>.Failure(request.GetErrorMessages());

            var order = await _orderRepository.GetByIdAsync(request.OrderId).ConfigureAwait(false);
            if (order is null)
            {
                _notificator.HandleNotification(new("Order not found."));
                return Response<CancelOrderProcessingReverseStockResponse>.Failure(_notificator.GetNotifications(), code: 404);
            }

            var listItems = new List<ItemDto>();
            foreach (var item in order.OrderItems)
            {
                listItems.Add(new ItemDto(item.ProductId, item.Quantity));
            }

            order.AddEvent(new OrderProcessingCanceledIntegrationEvent(order.Id, order.CustomerId, new OrderProductsListDto(order.Id, listItems)));
            order.DraftOrder();

            _orderRepository.Update(order);
            return await PersistDataAsync();
        }

        private async Task<Response<CancelOrderProcessingReverseStockResponse>> PersistDataAsync()
        {
            if (await _orderRepository.UnitOfWork.CommitAsync())
            {
                _notificator.HandleNotification(new("Fail to persist data."));
                return Response<CancelOrderProcessingReverseStockResponse>.Failure(_notificator.GetNotifications());
            }

            return Response<CancelOrderProcessingReverseStockResponse>.Success(default);
        }
    }
}