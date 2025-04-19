using SalesSystem.Sales.Application.Mappers;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Events.IntegrationEvents.Orders;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Commands.Orders.Start
{
    public sealed class StartOrderHandler(IOrderRepository orderRepository,
                                          INotificator notificator)
                                        : CommandHandler<StartOrderCommand, StartOrderResponse>(notificator)
    {
        public override async Task<Response<StartOrderResponse>> ExecuteAsync(StartOrderCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new StartOrderValidation(), request))
                return Response<StartOrderResponse>.Failure(GetNotifications());

            var order = await orderRepository.GetDraftOrderByCustomerIdAsync(request.CustomerId);
            if (order is null)
            {
                Notify("Order not found.");
                return Response<StartOrderResponse>.Failure(GetNotifications());
            }
            order.StartOrder();

            order.AddEvent(new StartedOrderIntegrationEvent(order.Id, order.CustomerId, order.Price,
                OrderMappers.MapFromEntityToOrderProductsListDTO([.. order.OrderItems], order.Id)));

            orderRepository.Update(order);
            return await PersistDataAsync();
        }

        private async Task<Response<StartOrderResponse>> PersistDataAsync()
        {
            if (!await orderRepository.UnitOfWork.CommitAsync())
            {
                Notify("Fail to persist data.");
                return Response<StartOrderResponse>.Failure(GetNotifications());
            }

            return Response<StartOrderResponse>.Success(default);
        }
    }
}