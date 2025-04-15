using MidR.Interfaces;
using SalesSystem.Catalog.Domain.Interfaces.Services;
using SalesSystem.SharedKernel.Events.IntegrationEvents.Orders;

namespace SalesSystem.Catalog.Domain.Events.CanceledOrder
{
    public sealed class OrderProcessingCanceledIntegrationEventHandler(IStockService stockService) : INotificationHandler<OrderProcessingCanceledIntegrationEvent>
    {
        private readonly IStockService _stockService = stockService;

        public async Task ExecuteAsync(OrderProcessingCanceledIntegrationEvent notification, CancellationToken cancellationToken)
        {
            await _stockService.AddListStockAsync(notification.OrderProducts);
        }
    }
}