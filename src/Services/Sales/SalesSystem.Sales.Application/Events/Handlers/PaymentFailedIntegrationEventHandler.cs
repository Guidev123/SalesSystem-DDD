using MediatR;
using SalesSystem.Sales.Application.Commands.Orders.CancelProcessingReverseStock;
using SalesSystem.SharedKernel.Communication.Mediator;
using SalesSystem.SharedKernel.Messages.CommonMessages.IntegrationEvents.Payments;

namespace SalesSystem.Sales.Application.Events.Handlers
{
    public sealed class PaymentFailedIntegrationEventHandler(IMediatorHandler mediator) : INotificationHandler<PaymentFailedIntegrationEvent>
    {
        public async Task Handle(PaymentFailedIntegrationEvent notification, CancellationToken cancellationToken)
            => await mediator.SendCommand(new CancelOrderProcessingReverseStockCommand(notification.OrderId, notification.CustomerId));
    }
}
