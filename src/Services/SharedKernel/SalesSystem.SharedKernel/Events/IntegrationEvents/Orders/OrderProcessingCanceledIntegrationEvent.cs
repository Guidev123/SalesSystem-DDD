using SalesSystem.SharedKernel.DTOs;

namespace SalesSystem.SharedKernel.Events.IntegrationEvents.Orders
{
    public record OrderProcessingCanceledIntegrationEvent : IntegrationEvent
    {
        public OrderProcessingCanceledIntegrationEvent(Guid orderId, Guid customerId, OrderProductsListDto orderProducts)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
            OrderProducts = orderProducts;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        public OrderProductsListDto OrderProducts { get; }
    }
}