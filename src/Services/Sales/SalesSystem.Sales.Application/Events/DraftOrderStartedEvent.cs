using SalesSystem.SharedKernel.Events;

namespace SalesSystem.Sales.Application.Events
{
    public record DraftOrderStartedEvent : Event
    {
        public DraftOrderStartedEvent(
            Guid orderId,
            Guid customerId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
    }
}