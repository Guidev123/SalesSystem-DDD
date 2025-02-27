using MediatR;

namespace SalesSystem.Catalog.Application.Queries.Products.GetAll
{
    public record GetAllProductsQuery(int PageNumber, int PageSize) : IRequest<GetAllProductsResponse>;
}
