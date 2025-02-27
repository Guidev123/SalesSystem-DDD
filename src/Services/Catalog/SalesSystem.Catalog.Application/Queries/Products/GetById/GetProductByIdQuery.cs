using MediatR;

namespace SalesSystem.Catalog.Application.Queries.Products.GetById
{
    public record GetProductByIdQuery(Guid Id) : IRequest<GetProductByIdResponse>;
}
