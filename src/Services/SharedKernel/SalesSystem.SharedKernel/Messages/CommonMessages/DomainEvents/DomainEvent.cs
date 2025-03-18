using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.SharedKernel.Messages.CommonMessages.DomainEvents
{
    public abstract record DomainEvent : Event
    {
        protected DomainEvent(Guid aggregateId) => AggregateId = aggregateId;
    }
}
