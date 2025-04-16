using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Catalog.Application.Queries.Categories.GetAll
{
    public record GetAllCategoriesQuery(int PageNumber, int PageSize) : IPagedQuery<GetAllCategoriesResponse>;
}