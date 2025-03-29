using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Sales.Application.Commands.Orders.UpdateOrderItem
{
    public record UpdateOrderItemCommand : Command<UpdateOrderItemResponse>
    {
        public UpdateOrderItemCommand(Guid orderId, Guid productId, int quantity)
        {
            AggregateId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }

        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; }
        public Guid ProductId { get; }
        public int Quantity { get; }

        public void SetCustomerId(Guid customerId)
            => CustomerId = customerId;

        public override bool IsValid()
        {
            SetValidationResult(new UpdateOrderItemValidation().Validate(this));
            return ValidationResult!.IsValid;
        }
    }
}