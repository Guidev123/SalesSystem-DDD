using MidR.Interfaces;
using SalesSystem.Sales.Application.Commands.Orders.CancelProcessing;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Events.IntegrationEvents.Orders;

namespace SalesSystem.Sales.Application.Events.Handlers
{
    public sealed class DebitOrderItemInStockFailedIntegrationEventHandler(IMediatorHandler mediator)
                                                                         : INotificationHandler<DebitOrderItemInStockFailedIntegrationEvent>
    {
        public async Task ExecuteAsync(DebitOrderItemInStockFailedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            await mediator.SendCommand(new CancelOrderProcessingCommand(notification.OrderId, notification.CustomerId));
        }
    }
}