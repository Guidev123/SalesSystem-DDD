using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Catalog.Application.Queries.Products.GetById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResponse>;
}
