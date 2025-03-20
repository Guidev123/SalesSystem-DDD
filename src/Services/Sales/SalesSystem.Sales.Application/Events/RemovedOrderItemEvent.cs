using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Sales.Application.Events
{
    public record RemovedOrderItemEvent : Event
    {
        public RemovedOrderItemEvent(Guid orderId, Guid customerId, Guid productId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
            ProductId = productId;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        public Guid ProductId { get; }
    }
}