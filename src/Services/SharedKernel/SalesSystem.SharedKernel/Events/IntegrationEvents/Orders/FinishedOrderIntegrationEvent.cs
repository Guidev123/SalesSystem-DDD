namespace SalesSystem.SharedKernel.Events.IntegrationEvents.Orders
{
    public record FinishedOrderIntegrationEvent : IntegrationEvent
    {
        public FinishedOrderIntegrationEvent(Guid orderId)
        {
            AggregateId = orderId;
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}