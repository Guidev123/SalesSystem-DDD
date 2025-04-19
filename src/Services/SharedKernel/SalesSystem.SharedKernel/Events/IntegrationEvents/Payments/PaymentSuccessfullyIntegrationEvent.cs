namespace SalesSystem.SharedKernel.Events.IntegrationEvents.Payments
{
    public record PaymentSuccessfullyIntegrationEvent : IntegrationEvent
    {
        public PaymentSuccessfullyIntegrationEvent(Guid orderId, Guid customerId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
    }
}