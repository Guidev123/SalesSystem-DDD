using FluentValidation.Results;
using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Products.Update
{
    public record UpdateProductCommand(Guid Id, string? Description, string? Image, decimal? Price) : IRequest<Response<UpdateProductResponse>>
    {
        public ValidationResult Validate(UpdateProductCommand command)
            => new UpdateProductValidation().Validate(command);
    }
}
