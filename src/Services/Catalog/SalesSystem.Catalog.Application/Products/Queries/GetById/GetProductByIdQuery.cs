using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Catalog.Application.Products.Queries.GetById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResponse>;
}