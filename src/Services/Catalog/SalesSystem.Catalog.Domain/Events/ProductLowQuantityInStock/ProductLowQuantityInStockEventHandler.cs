using MidR.Interfaces;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;

namespace SalesSystem.Catalog.Domain.Events.ProductLowQuantityInStock
{
    public sealed class ProductLowQuantityInStockEventHandler(IProductRepository productRepository)
                                          : INotificationHandler<ProductLowQuantityInStockEvent>
    {
        public Task ExecuteAsync(ProductLowQuantityInStockEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Open ticket to buy more products...
            return Task.CompletedTask;
        }
    }
}