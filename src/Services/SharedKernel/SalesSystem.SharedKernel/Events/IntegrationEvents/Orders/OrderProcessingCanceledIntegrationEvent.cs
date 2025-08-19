using SalesSystem.SharedKernel.Models;

namespace SalesSystem.SharedKernel.Events.IntegrationEvents.Orders
{
    public record OrderProcessingCanceledIntegrationEvent : IntegrationEvent
    {
        public OrderProcessingCanceledIntegrationEvent(Guid orderId, Guid customerId, OrderProductsList orderProducts)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
            OrderProducts = orderProducts;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        public OrderProductsList OrderProducts { get; }
    }
}