using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Sales.Application.Commands.Orders.RemoveOrderItem
{
    public record RemoveOrderItemCommand : Command<RemoveOrderItemResponse>
    {
        public RemoveOrderItemCommand(Guid customerId, Guid orderId, Guid productId)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
        }

        public Guid CustomerId { get; }
        public Guid OrderId { get; }
        public Guid ProductId { get; }

        public override bool IsValid()
        {
            SetValidationResult(new RemoveOrderItemValidation().Validate(this));
            return ValidationResult.IsValid;
        }
    }
}