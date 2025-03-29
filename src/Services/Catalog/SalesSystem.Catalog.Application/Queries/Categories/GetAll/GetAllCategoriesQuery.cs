using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Catalog.Application.Queries.Categories.GetAll
{
    public record GetAllCategoriesQuery() : IQuery<GetAllCategoriesResponse>;
}