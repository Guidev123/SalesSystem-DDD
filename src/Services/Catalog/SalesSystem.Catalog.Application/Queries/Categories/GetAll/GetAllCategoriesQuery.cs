using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Catalog.Application.Queries.Categories.GetAll
{
    public record GetAllCategoriesQuery() : IPagedQuery<GetAllCategoriesResponse>;
}