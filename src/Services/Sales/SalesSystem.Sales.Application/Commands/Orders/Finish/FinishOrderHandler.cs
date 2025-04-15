using MidR.Interfaces;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Events.IntegrationEvents.Orders;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Commands.Orders.Finish
{
    public sealed class FinishOrderHandler(IOrderRepository orderRepository,
                                           INotificator notificator)
                                         : IRequestHandler<FinishOrderCommand, Response<FinishOrderResponse>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly INotificator _notificator = notificator;

        public async Task<Response<FinishOrderResponse>> ExecuteAsync(FinishOrderCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<FinishOrderResponse>.Failure(request.GetErrorMessages());

            var order = await _orderRepository.GetByIdAsync(request.OrderId).ConfigureAwait(false);
            if (order is null)
            {
                _notificator.HandleNotification(new("Order not found."));
                return Response<FinishOrderResponse>.Failure(_notificator.GetNotifications(), code: 404);
            }

            order.PayOrder();
            _orderRepository.Update(order);

            order.AddEvent(new FinishedOrderIntegrationEvent(request.OrderId));

            return await PersistDataAsync();
        }

        private async Task<Response<FinishOrderResponse>> PersistDataAsync()
        {
            if (await _orderRepository.UnitOfWork.CommitAsync())
            {
                _notificator.HandleNotification(new("Fail to persist data."));
                return Response<FinishOrderResponse>.Failure(_notificator.GetNotifications());
            }

            return Response<FinishOrderResponse>.Success(default);
        }
    }
}