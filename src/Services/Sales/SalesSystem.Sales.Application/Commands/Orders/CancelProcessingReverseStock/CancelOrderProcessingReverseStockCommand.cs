using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Sales.Application.Commands.Orders.CancelProcessingReverseStock
{
    public record CancelOrderProcessingReverseStockCommand : Command<CancelOrderProcessingReverseStockResponse>
    {
        public CancelOrderProcessingReverseStockCommand(Guid orderId, Guid customerId)
        {
            AggregateId = orderId;
            OrderId = orderId;
            CustomerId = customerId;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        public override bool IsValid()
        {
            SetValidationResult(new CancelOrderProcessingReverseStockValidation().Validate(this));
            return ValidationResult!.IsValid;
        }
    }
}