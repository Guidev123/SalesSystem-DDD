using MediatR;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Messages.CommonMessages.IntegrationEvents.Order;

namespace SalesSystem.Sales.Application.Events.Handlers
{
    public sealed class DebitOrderItemInStockFailedIntegrationEventHandler(IOrderRepository orderRepository)
                                                                         : INotificationHandler<DebitOrderItemInStockFailedIntegrationEvent>
    {
        public async Task Handle(DebitOrderItemInStockFailedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(notification.OrderId);
            if (order is not null)
            {
                order.CancelOrder();
            }
        }
    }
}
