using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Catalog.Domain.Events
{
    public record ProductLowQuantityInStockEvent : DomainEvent
    {
        public ProductLowQuantityInStockEvent(int currentQuantityInStock, Guid aggregateId)
            : base(aggregateId) => CurrentQuantityInStock = currentQuantityInStock;

        public int CurrentQuantityInStock { get; private set; }
    }
}
