using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Sales.Application.Commands.Orders.AddOrderItem
{
    public record AddOrderItemCommand : Command<AddOrderItemResponse>
    {
        public AddOrderItemCommand(Guid productId, string name, int quantity, decimal unitPrice)
        {
            ProductId = productId;
            Name = name;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public Guid CustomerId { get; private set; }
        public Guid ProductId { get; }
        public string Name { get; } = string.Empty;
        public int Quantity { get; }
        public decimal UnitPrice { get; }

        public void SetCustomerId(Guid customerId)
        {
            CustomerId = customerId;
            AggregateId = customerId;
        }
    }
}