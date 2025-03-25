using SalesSystem.SharedKernel.Communication.DTOs;

namespace SalesSystem.SharedKernel.Messages.CommonMessages.IntegrationEvents.Orders
{
    public record OrderProcessingCanceledIntegrationEvent : IntegrationEvent
    {
        public OrderProcessingCanceledIntegrationEvent(Guid orderId, Guid customerId, OrderProductsListDTO orderProducts)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
            OrderProducts = orderProducts;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        public OrderProductsListDTO OrderProducts { get; }
    }
}
