using MediatR;

namespace SalesSystem.Catalog.Application.Queries.Categories.GetAll
{
    public record GetAllCategoriesQuery(int PageNumber, int PageSize) : IRequest<GetAllCategoriesResponse>;
}
