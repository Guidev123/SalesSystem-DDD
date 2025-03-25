using MediatR;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Commands.Orders.CancelProcessing
{
    public sealed class CancelOrderProcessingHandler(IOrderRepository orderRepository,
                                                                 INotificator notificator)
                                                               : IRequestHandler<CancelOrderProcessingCommand, Response<CancelOrderProcessingResponse>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly INotificator _notificator = notificator;

        public async Task<Response<CancelOrderProcessingResponse>> Handle(CancelOrderProcessingCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<CancelOrderProcessingResponse>.Failure(request.GetErrorMessages());

            var order = await _orderRepository.GetByIdAsync(request.OrderId).ConfigureAwait(false);
            if (order is null)
            {
                _notificator.HandleNotification(new("Order not found."));
                return Response<CancelOrderProcessingResponse>.Failure(_notificator.GetNotifications(), code: 404);
            }

            order.DraftOrder();
            _orderRepository.Update(order);

            return await PersistDataAsync();
        }

        private async Task<Response<CancelOrderProcessingResponse>> PersistDataAsync()
        {
            if (await _orderRepository.UnitOfWork.CommitAsync())
            {
                _notificator.HandleNotification(new("Fail to persist data."));
                return Response<CancelOrderProcessingResponse>.Failure(_notificator.GetNotifications());
            }

            return Response<CancelOrderProcessingResponse>.Success(default);
        }
    }
}
