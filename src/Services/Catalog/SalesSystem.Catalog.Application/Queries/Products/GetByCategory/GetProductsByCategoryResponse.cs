using SalesSystem.Catalog.Application.DTOs;

namespace SalesSystem.Catalog.Application.Queries.Products.GetByCategory
{
    public record GetProductsByCategoryResponse(IEnumerable<ProductDTO> Products);
}