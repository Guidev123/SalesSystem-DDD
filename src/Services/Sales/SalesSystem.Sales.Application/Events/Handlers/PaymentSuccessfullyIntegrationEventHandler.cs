using MediatR;
using SalesSystem.Sales.Application.Commands.Orders.Finish;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Events.IntegrationEvents.Payments;

namespace SalesSystem.Sales.Application.Events.Handlers
{
    public sealed class PaymentSuccessfullyIntegrationEventHandler(IMediatorHandler mediator) : INotificationHandler<PaymentSuccessfullyIntegrationEvent>
    {
        public async Task Handle(PaymentSuccessfullyIntegrationEvent notification, CancellationToken cancellationToken)
            => await mediator.SendCommand(new FinishOrderCommand(notification.OrderId, notification.CustomerId));
    }
}