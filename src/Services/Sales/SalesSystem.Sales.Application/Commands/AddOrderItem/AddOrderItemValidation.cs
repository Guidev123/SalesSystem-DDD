using FluentValidation;

namespace SalesSystem.Sales.Application.Commands.AddOrderItem
{
    public sealed class AddOrderItemValidation : AbstractValidator<AddOrderItemCommand>
    {
        public AddOrderItemValidation()
        {
            RuleFor(x => x.CustomerId)
                 .NotEqual(Guid.Empty)
                 .WithMessage("Customer id is not valid.");

            RuleFor(x => x.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Product id is not valid.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(100)
                .WithMessage("Product name must be at most 100 characters long.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.")
                .LessThan(15)
                .WithMessage("Quantity must be less than 15.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0)
                .WithMessage("Unit price must be greater than zero.");
        }
    }
}
