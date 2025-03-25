using FluentValidation;

namespace SalesSystem.Sales.Application.Commands.Orders.Finish
{
    public sealed class FinishOrderValidation : AbstractValidator<FinishOrderCommand>
    {
        public FinishOrderValidation()
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
