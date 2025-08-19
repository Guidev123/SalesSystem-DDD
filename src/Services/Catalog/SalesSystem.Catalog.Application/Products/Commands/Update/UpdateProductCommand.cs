using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Catalog.Application.Products.Commands.Update
{
    public record UpdateProductCommand(Guid Id, string? Description, string? Image, decimal? Price) : Command<UpdateProductResponse>
    {
    }
}