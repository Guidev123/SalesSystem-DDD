using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.SharedKernel.DomainObjects
{
    public abstract class DomainEvent : Event
    {
        protected DomainEvent(Guid aggregateId) => AggregateId = aggregateId;
    }
}
