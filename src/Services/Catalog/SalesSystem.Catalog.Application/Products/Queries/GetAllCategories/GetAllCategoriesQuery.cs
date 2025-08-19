using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Catalog.Application.Products.Queries.GetAllCategories
{
    public record GetAllCategoriesQuery(int PageNumber, int PageSize) : IPagedQuery<GetAllCategoriesResponse>;
}