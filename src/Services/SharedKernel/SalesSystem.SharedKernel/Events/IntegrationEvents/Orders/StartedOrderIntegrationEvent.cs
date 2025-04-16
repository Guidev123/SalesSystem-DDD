using SalesSystem.SharedKernel.DTOs;
using SalesSystem.SharedKernel.Events.IntegrationEvents;

namespace SalesSystem.SharedKernel.Events.IntegrationEvents.Orders
{
    public record StartedOrderIntegrationEvent : IntegrationEvent
    {
        public StartedOrderIntegrationEvent(Guid orderId, Guid customerId, decimal total, OrderProductsListDto orderProductsList)
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
        public OrderProductsListDto OrderProductsList { get; }
    }
}