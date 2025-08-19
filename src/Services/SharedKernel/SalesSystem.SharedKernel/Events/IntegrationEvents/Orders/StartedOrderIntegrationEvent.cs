using SalesSystem.SharedKernel.Models;

namespace SalesSystem.SharedKernel.Events.IntegrationEvents.Orders
{
    public record StartedOrderIntegrationEvent : IntegrationEvent
    {
        public StartedOrderIntegrationEvent(Guid orderId, Guid customerId, decimal total, OrderProductsList orderProductsList)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
            Total = total;
            OrderProductsList = orderProductsList;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        public decimal Total { get; }
        public OrderProductsList OrderProductsList { get; }
    }
}