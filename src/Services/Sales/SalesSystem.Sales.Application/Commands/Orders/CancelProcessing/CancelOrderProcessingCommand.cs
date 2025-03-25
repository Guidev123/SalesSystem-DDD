using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Sales.Application.Commands.Orders.CancelProcessing
{
    public record CancelOrderProcessingCommand : Command<CancelOrderProcessingResponse>
    {
        public CancelOrderProcessingCommand(Guid orderId, Guid customerId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        public override bool IsValid()
        {
            SetValidationResult(new CancelOrderProcessingValidation().Validate(this));
            return ValidationResult.IsValid;
        }
    }
}
