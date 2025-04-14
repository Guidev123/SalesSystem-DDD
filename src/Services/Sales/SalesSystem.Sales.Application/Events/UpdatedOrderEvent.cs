using SalesSystem.SharedKernel.Events;

namespace SalesSystem.Sales.Application.Events
{
    public record UpdatedOrderEvent : Event
    {
        public UpdatedOrderEvent(Guid orderId,
                                 Guid customerId,
                                 decimal totalPrice)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
            TotalPrice = totalPrice;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        public decimal TotalPrice { get; }
    }
}