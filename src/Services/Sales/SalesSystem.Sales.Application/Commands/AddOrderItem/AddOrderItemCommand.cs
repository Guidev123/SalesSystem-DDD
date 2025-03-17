using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Sales.Application.Commands.AddOrderItem
{
    public record AddOrderItemCommand : Command<Guid>
    {
        public AddOrderItemCommand(Guid customerId, Guid productId, string name, int quantity, decimal unitPrice)
        {
            CustomerId = customerId;
            ProductId = productId;
            Name = name;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public Guid CustomerId { get; private set; }
        public Guid ProductId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public override bool IsValid()
        {
            SetValidationResult(new AddOrderItemValidation().Validate(this));
            return ValidationResult.IsValid;
        }
    }
}
