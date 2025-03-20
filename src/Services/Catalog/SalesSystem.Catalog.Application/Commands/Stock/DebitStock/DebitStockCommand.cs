using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Catalog.Application.Commands.Stock.DebitStock
{
    public record DebitStockCommand(Guid Id, int Quantity) : Command<DebitStockResponse>
    {
        public override bool IsValid()
        {
            SetValidationResult(new DebitStockValidation().Validate(this));
            return ValidationResult.IsValid;
        }
    }
}