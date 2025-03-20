using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Sales.Application.Events
{
    public record UpdatedOrderItemEvent : Event
    {
        public UpdatedOrderItemEvent(Guid orderId,
                                 Guid customerId,
                                 decimal totalPrice,
                                 int quantity)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
            TotalPrice = totalPrice;
            Quantity = quantity;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        public decimal TotalPrice { get; }
        public int Quantity { get; }
    }
}