using MediatR;

namespace SalesSystem.Catalog.Application.Queries.Products.GetByCategory
{
    public record GetProductByCategoryQuery(int Code) : IRequest<GetProductByCategoryResponse>;
}
