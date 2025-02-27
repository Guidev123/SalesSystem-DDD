using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Categories.GetAll
{
    public record GetAllCategoriesQuery(int PageNumber, int PageSize) : IRequest<PagedResponse<GetAllCategoriesResponse>>;
}
