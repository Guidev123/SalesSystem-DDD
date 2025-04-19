using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Catalog.Application.Commands.Products.Update
{
    public record UpdateProductCommand(Guid Id, string? Description, string? Image, decimal? Price) : Command<UpdateProductResponse>
    {
    }
}