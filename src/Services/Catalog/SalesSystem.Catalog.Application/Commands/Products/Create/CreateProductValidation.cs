using FluentValidation;

namespace SalesSystem.Catalog.Application.Commands.Products.Create
{
    public sealed class CreateProductValidation : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidation()
        {

        }
    }
}
