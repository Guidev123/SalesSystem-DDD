using SalesSystem.Catalog.Application.DTOs;

namespace SalesSystem.Catalog.Application.Queries.Products.GetAll
{
    public record GetAllProductsResponse(IEnumerable<ProductDTO> Products);
}
