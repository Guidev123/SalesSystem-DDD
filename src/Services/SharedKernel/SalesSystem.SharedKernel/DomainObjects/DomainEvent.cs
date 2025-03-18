using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.SharedKernel.DomainObjects
{
    public abstract record DomainEvent : Event
    {
        protected DomainEvent(Guid aggregateId) => AggregateId = aggregateId;
    }
}
