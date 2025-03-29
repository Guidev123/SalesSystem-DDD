using MediatR;
using SalesSystem.Sales.Application.Commands.Orders.CancelProcessing;
using SalesSystem.SharedKernel.Communication.Mediator;
using SalesSystem.SharedKernel.Messages.CommonMessages.IntegrationEvents.Orders;

namespace SalesSystem.Sales.Application.Events.Handlers
{
    public sealed class DebitOrderItemInStockFailedIntegrationEventHandler(IMediatorHandler mediator)
                                                                         : INotificationHandler<DebitOrderItemInStockFailedIntegrationEvent>
    {
        public async Task Handle(DebitOrderItemInStockFailedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            await mediator.SendCommand(new CancelOrderProcessingCommand(notification.OrderId, notification.CustomerId));
        }
    }
}