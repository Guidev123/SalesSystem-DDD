using SalesSystem.Catalog.Application.DTOs;

namespace SalesSystem.Catalog.Application.Queries.Categories.GetAll
{
    public record GetAllCategoriesResponse(IEnumerable<CategoryDTO> Categories);
}