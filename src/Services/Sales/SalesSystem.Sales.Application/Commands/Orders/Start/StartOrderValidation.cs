using FluentValidation;

namespace SalesSystem.Sales.Application.Commands.Orders.Start
{
    public sealed class StartOrderValidation : AbstractValidator<StartOrderCommand>
    {
        public StartOrderValidation()
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
