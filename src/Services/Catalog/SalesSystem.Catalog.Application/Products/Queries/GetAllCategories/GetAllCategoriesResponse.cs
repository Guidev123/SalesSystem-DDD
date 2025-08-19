using SalesSystem.Catalog.Application.Products.DTOs;

namespace SalesSystem.Catalog.Application.Products.Queries.GetAllCategories
{
    public record GetAllCategoriesResponse(IEnumerable<CategoryDto> Categories);
}