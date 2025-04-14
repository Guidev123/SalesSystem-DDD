using SalesSystem.SharedKernel.Events.DomainEvents;

namespace SalesSystem.Catalog.Domain.Events.ProductLowQuantityInStock
{
    public record ProductLowQuantityInStockEvent : DomainEvent
    {
        public ProductLowQuantityInStockEvent(int currentQuantityInStock, Guid aggregateId)
            : base(aggregateId) => CurrentQuantityInStock = currentQuantityInStock;

        public int CurrentQuantityInStock { get; private set; }
    }
}