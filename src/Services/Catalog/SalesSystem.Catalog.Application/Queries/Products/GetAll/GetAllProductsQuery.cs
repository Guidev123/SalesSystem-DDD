using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Products.GetAll
{
    public record GetAllProductsQuery(int PageNumber, int PageSize) : IRequest<PagedResponse<GetAllProductsResponse>>;
}
