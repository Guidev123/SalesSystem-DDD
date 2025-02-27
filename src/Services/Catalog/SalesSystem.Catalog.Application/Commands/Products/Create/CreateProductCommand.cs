using MediatR;
using SalesSystem.Catalog.Application.DTOs;

namespace SalesSystem.Catalog.Application.Commands.Products.Create
{
    public record class CreateProductCommand(string Name, string Description, string Image,
        decimal Price, int QuantityInStock, decimal Height,
        decimal Width, decimal Depth, IEnumerable<CategoryDTO> Categories) : IRequest<CreateProductResponse>;
}
