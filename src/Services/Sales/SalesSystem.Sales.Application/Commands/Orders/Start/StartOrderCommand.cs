using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Sales.Application.Commands.Orders.Start
{
    public record StartOrderCommand : Command<StartOrderResponse>
    {
        public StartOrderCommand(Guid orderId, Guid customerId, decimal totalPrice)
        {
            OrderId = orderId;
            CustomerId = customerId;
            TotalPrice = totalPrice;
        }

        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        public decimal TotalPrice { get; }

        public override bool IsValid()
        {
            SetValidationResult(new StartOrderValidation().Validate(this));
            return ValidationResult.IsValid;
        }
    }
}
