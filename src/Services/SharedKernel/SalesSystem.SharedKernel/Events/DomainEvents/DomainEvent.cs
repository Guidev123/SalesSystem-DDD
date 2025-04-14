namespace SalesSystem.SharedKernel.Events.DomainEvents
{
    public abstract record DomainEvent : Event
    {
        protected DomainEvent(Guid aggregateId) => AggregateId = aggregateId;
    }
}