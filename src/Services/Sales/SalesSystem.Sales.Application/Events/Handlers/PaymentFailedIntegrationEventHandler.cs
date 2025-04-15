using MidR.Interfaces;
using SalesSystem.Sales.Application.Commands.Orders.CancelProcessingReverseStock;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Events.IntegrationEvents.Payments;

namespace SalesSystem.Sales.Application.Events.Handlers
{
    public sealed class PaymentFailedIntegrationEventHandler(IMediatorHandler mediator) : INotificationHandler<PaymentFailedIntegrationEvent>
    {
        public async Task ExecuteAsync(PaymentFailedIntegrationEvent notification, CancellationToken cancellationToken)
            => await mediator.SendCommand(new CancelOrderProcessingReverseStockCommand(notification.OrderId, notification.CustomerId));
    }
}