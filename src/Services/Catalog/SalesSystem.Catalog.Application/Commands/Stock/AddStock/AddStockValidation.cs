using FluentValidation;

namespace SalesSystem.Catalog.Application.Commands.Stock.AddStock
{
    public sealed class AddStockValidation : AbstractValidator<AddStockCommand>
    {
        public AddStockValidation()
        {
            RuleFor(x => x.Id).NotEmpty().NotEqual(Guid.Empty).WithMessage("Id cannot be empty.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
