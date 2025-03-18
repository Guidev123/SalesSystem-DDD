using FluentValidation.Results;
using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Catalog.Application.Commands.Products.Update
{
    public record UpdateProductCommand(Guid Id, string? Description, string? Image, decimal? Price) : Command<UpdateProductResponse>
    {
        public override bool IsValid()
        {
            SetValidationResult(new UpdateProductValidation().Validate(this));
            return ValidationResult.IsValid;
        }
    }
}
