using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Sales.Application.Commands.Orders.Finish
{
    public record FinishOrderCommand : Command<FinishOrderResponse>
    {
        public FinishOrderCommand(Guid orderId, Guid customerId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        public override bool IsValid()
        {
            SetValidationResult(new FinishOrderValidation().Validate(this));
            return ValidationResult!.IsValid;
        }
    }
}