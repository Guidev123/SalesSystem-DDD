using SalesSystem.Catalog.Application.Products.DTOs;

namespace SalesSystem.Catalog.Application.Products.Queries.GetAll
{
    public record GetAllProductsResponse(IEnumerable<ProductDto> Products);
}