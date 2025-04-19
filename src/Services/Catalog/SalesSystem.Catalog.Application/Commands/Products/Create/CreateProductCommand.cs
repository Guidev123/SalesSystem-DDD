using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Catalog.Application.Commands.Products.Create
{
    public record CreateProductCommand(string Name, string Description, string Image,
        decimal Price, decimal Height,
        decimal Width, decimal Depth, Guid CategoryId) : Command<CreateProductResponse>
    {
    }
}