using SalesSystem.Catalog.Application.DTOs;

namespace SalesSystem.Catalog.Application.Queries.Products.GetByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<ProductDTO> Products);
}
