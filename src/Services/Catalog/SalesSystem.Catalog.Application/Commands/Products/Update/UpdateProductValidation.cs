using FluentValidation;

namespace SalesSystem.Catalog.Application.Commands.Products.Update
{
    public sealed class UpdateProductValidation : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidation()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("Product description must not exceed 500 characters.");

            RuleFor(p => p.Image)
                .NotEmpty().WithMessage("Product image is required.");

            RuleFor(p => p.Price)
                .GreaterThan(0).When(p => p.Price.HasValue)
                .WithMessage("Product price must be greater than zero.");
        }
    }
}
