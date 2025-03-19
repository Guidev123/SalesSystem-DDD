using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Sales.Application.Commands.Orders.UpdateOrderItem
{
    public record UpdateOrderItemCommand : Command<UpdateOrderItemResponse>
    {
        public UpdateOrderItemCommand(Guid customerId, Guid orderId, Guid productId, int quantity)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            ProductId = productId;
            Quantity = quantity;
        }

        public Guid CustomerId { get; }
        public Guid OrderId { get; }
        public Guid ProductId { get; }
        public int Quantity { get; }

        public override bool IsValid()
        {
            SetValidationResult(new UpdateOrderItemValidation().Validate(this));
            return ValidationResult.IsValid;
        }
    }
}
