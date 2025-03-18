using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Catalog.Application.Commands.Stock.AddStock
{
    public record AddStockCommand(Guid Id, int Quantity) : Command<AddStockResponse>
    {
        public override bool IsValid()
        {
            SetValidationResult(new AddStockValidation().Validate(this));
            return ValidationResult.IsValid;
        }
    }
}
