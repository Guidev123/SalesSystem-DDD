using MediatR;
using SalesSystem.Sales.Application.Mappers;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Events.IntegrationEvents.Orders;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Commands.Orders.Start
{
    public sealed class StartOrderHandler(IOrderRepository orderRepository,
                                          INotificator notificator)
                                        : IRequestHandler<StartOrderCommand, Response<StartOrderResponse>>
    {
        private readonly INotificator _notificator = notificator;
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<Response<StartOrderResponse>> Handle(StartOrderCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<StartOrderResponse>.Failure(request.GetErrorMessages());

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(request.CustomerId);
            if (order is null)
            {
                _notificator.HandleNotification(new("Order not found."));
                return Response<StartOrderResponse>.Failure(_notificator.GetNotifications());
            }
            order.StartOrder();

            order.AddEvent(new StartedOrderIntegrationEvent(order.Id, order.CustomerId, order.Price,
                OrderMappers.MapFromEntityToOrderProductsListDTO([.. order.OrderItems], order.Id)));

            _orderRepository.Update(order);
            return await PersistDataAsync();
        }

        private async Task<Response<StartOrderResponse>> PersistDataAsync()
        {
            if (!await _orderRepository.UnitOfWork.CommitAsync())
            {
                _notificator.HandleNotification(new("Fail to persist data."));
                return Response<StartOrderResponse>.Failure(_notificator.GetNotifications());
            }

            return Response<StartOrderResponse>.Success(default);
        }
    }
}