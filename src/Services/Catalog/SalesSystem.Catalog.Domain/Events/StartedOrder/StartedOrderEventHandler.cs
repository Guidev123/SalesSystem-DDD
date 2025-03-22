using MediatR;
using SalesSystem.Catalog.Domain.Interfaces.Services;
using SalesSystem.SharedKernel.Communication.Mediator;
using SalesSystem.SharedKernel.Messages.CommonMessages.IntegrationEvents.Order;

namespace SalesSystem.Catalog.Domain.Events.StartedOrder
{
    public sealed class StartedOrderEventHandler : INotificationHandler<StartedOrderIntegrationEvent>
    {
        private readonly IStockService _stockService;
        private readonly IMediatorHandler _mediatorHandler;

        public StartedOrderEventHandler(IStockService stockService, IMediatorHandler mediatorHandler)
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
                await _mediatorHandler.PublishEventAsync(new DebitOrderItemInStockFailedIntegrationEvent(notification.OrderId)).ConfigureAwait(false);
        }
    }
}
