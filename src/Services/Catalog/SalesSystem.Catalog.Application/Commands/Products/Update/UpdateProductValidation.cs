using FluentValidation;

namespace SalesSystem.Catalog.Application.Commands.Products.Update
{
    public sealed class UpdateProductValidation : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidation()
        {
            
        }
    }
}
