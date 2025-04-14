using SalesSystem.SharedKernel.Events.IntegrationEvents;

namespace SalesSystem.SharedKernel.Events.IntegrationEvents.Orders
{
    public record DebitOrderItemInStockConfirmedIntegrationEvent : IntegrationEvent
    {
        public DebitOrderItemInStockConfirmedIntegrationEvent(Guid orderId, Guid customerId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
    }
}