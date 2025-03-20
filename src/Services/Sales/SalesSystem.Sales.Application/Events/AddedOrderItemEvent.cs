using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Sales.Application.Events
{
    public record AddedOrderItemEvent : Event
    {
        public AddedOrderItemEvent(Guid orderId,
                                   Guid customerId,
                                   Guid productId,
                                   decimal unitPrice,
                                   int quantity,
                                   string productName)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
            ProductId = productId;
            UnitPrice = unitPrice;
            Quantity = quantity;
            ProductName = productName;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        public Guid ProductId { get; }
        public string ProductName { get; } = string.Empty;
        public decimal UnitPrice { get; }
        public int Quantity { get; }
    }
}