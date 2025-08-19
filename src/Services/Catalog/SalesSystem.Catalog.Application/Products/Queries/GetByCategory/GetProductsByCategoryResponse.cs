using SalesSystem.Catalog.Application.Products.DTOs;

namespace SalesSystem.Catalog.Application.Products.Queries.GetByCategory
{
    public record GetProductsByCategoryResponse(IEnumerable<ProductDto> Products);
}