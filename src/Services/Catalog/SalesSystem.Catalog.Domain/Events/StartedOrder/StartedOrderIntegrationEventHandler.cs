using MediatR;
using SalesSystem.Catalog.Domain.Interfaces.Services;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Events.IntegrationEvents.Orders;

namespace SalesSystem.Catalog.Domain.Events.StartedOrder
{
    public sealed class StartedOrderIntegrationEventHandler : INotificationHandler<StartedOrderIntegrationEvent>
    {
        private readonly IStockService _stockService;
        private readonly IMediatorHandler _mediatorHandler;

        public StartedOrderIntegrationEventHandler(IStockService stockService, IMediatorHandler mediatorHandler)
        {
            _stockService = stockService;
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(StartedOrderIntegrationEvent notification, CancellationToken cancellationToken)
        {
            var resultStock = await _stockService.DebitListStockAsync(notification.OrderProductsList).ConfigureAwait(false);
            if (resultStock)
                await _mediatorHandler.PublishEventAsync(new DebitOrderItemInStockConfirmedIntegrationEvent(notification.OrderId, notification.CustomerId)).ConfigureAwait(false);
            else
                await _mediatorHandler.PublishEventAsync(new DebitOrderItemInStockFailedIntegrationEvent(notification.OrderId, notification.CustomerId)).ConfigureAwait(false);
        }
    }
}