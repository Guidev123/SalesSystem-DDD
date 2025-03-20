using FluentValidation;

namespace SalesSystem.Catalog.Application.Commands.Stock.DebitStock
{
    public sealed class DebitStockValidation : AbstractValidator<DebitStockCommand>
    {
        public DebitStockValidation()
        {
            RuleFor(x => x.Id).NotEmpty().NotEqual(Guid.Empty).WithMessage("Id cannot be empty.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}