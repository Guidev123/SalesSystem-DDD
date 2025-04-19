namespace SalesSystem.SharedKernel.Events.IntegrationEvents.Orders
{
    public record DebitOrderItemInStockFailedIntegrationEvent : IntegrationEvent
    {
        public DebitOrderItemInStockFailedIntegrationEvent(Guid orderId, Guid customerId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
    }
}