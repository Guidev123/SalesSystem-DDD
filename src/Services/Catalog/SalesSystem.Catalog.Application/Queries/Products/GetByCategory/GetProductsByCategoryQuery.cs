using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Products.GetByCategory
{
    public record GetProductsByCategoryQuery(int PageNumber, int PageSize, int Code) : IRequest<PagedResponse<GetProductsByCategoryResponse>>;
}
