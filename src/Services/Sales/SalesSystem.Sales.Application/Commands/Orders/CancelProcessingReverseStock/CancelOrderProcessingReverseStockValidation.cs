using FluentValidation;

namespace SalesSystem.Sales.Application.Commands.Orders.CancelProcessingReverseStock
{
    public sealed class CancelOrderProcessingReverseStockValidation : AbstractValidator<CancelOrderProcessingReverseStockCommand>
    {
        public CancelOrderProcessingReverseStockValidation()
        {
            RuleFor(x => x.CustomerId)
                 .NotEqual(Guid.Empty)
                 .WithMessage("Customer id is not valid.");

            RuleFor(x => x.OrderId)
                .NotEqual(Guid.Empty)
                .WithMessage("Order id is not valid.");
        }
    }
}
