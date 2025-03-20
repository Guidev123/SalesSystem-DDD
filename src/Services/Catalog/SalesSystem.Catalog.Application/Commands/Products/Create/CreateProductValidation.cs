using FluentValidation;

namespace SalesSystem.Catalog.Application.Commands.Products.Create
{
    public sealed class CreateProductValidation : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidation()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Product description is required.")
                .MaximumLength(500).WithMessage("Product description must not exceed 500 characters.");

            RuleFor(p => p.Image)
                .NotEmpty().WithMessage("Product image is required.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Product price must be greater than zero.");

            RuleFor(p => p.QuantityInStock)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity in stock cannot be negative.");

            RuleFor(p => p.Height)
                .GreaterThan(0).WithMessage("Product height must be greater than zero.");

            RuleFor(p => p.Width)
                .GreaterThan(0).WithMessage("Product width must be greater than zero.");

            RuleFor(p => p.Depth)
                .GreaterThan(0).WithMessage("Product depth must be greater than zero.");

            RuleFor(p => p.CategoryId)
                .NotEmpty().WithMessage("Category ID is required.");
        }
    }
}