using MediatR;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;

namespace SalesSystem.Catalog.Domain.Events
{
    public sealed class ProductLowQuantityInStockEventHandler(IProductRepository productRepository)
                                          : INotificationHandler<ProductLowQuantityInStockEvent>
    {
        private readonly IProductRepository _productRepository = productRepository;

        public Task Handle(ProductLowQuantityInStockEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Open ticket to buy more products...
            return Task.CompletedTask;
        }
    }
}