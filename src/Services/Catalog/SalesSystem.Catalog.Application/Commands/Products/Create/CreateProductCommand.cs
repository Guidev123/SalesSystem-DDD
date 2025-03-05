using FluentValidation.Results;
using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Products.Create
{
    public record class CreateProductCommand(string Name, string Description, string Image,
        decimal Price, int QuantityInStock, decimal Height,
        decimal Width, decimal Depth, Guid CategoryId) : IRequest<Response<CreateProductResponse>>
    {
        public ValidationResult Validate(CreateProductCommand command)
            => new CreateProductValidation().Validate(command);
    }
}
