using MediatR;
using SalesSystem.Catalog.Application.DTOs;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Products.Update
{
    public record UpdateProductCommand(string Name, string Description, string Image,
        decimal Price, int QuantityInStock, decimal Height,
        decimal Width, decimal Depth, IEnumerable<CategoryDTO> Categories) : IRequest<Response<UpdateProductResponse>>;
}
