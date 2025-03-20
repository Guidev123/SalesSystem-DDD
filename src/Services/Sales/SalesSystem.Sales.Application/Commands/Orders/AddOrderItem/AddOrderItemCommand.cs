using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Sales.Application.Commands.Orders.AddOrderItem
{
    public record AddOrderItemCommand : Command<AddOrderItemResponse>
    {
        public AddOrderItemCommand(Guid customerId, Guid productId, string name, int quantity, decimal unitPrice)
        {
            AggregateId = customerId;
            CustomerId = customerId;
            ProductId = productId;
            Name = name;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public Guid CustomerId { get; }
        public Guid ProductId { get; }
        public string Name { get; } = string.Empty;
        public int Quantity { get; }
        public decimal UnitPrice { get; }

        public override bool IsValid()
        {
            SetValidationResult(new AddOrderItemValidation().Validate(this));
            return ValidationResult.IsValid;
        }
    }
}