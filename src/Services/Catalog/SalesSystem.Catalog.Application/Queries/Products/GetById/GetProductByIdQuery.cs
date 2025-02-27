using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Products.GetById
{
    public record GetProductByIdQuery(Guid Id) : IRequest<Response<GetProductByIdResponse>>;
}
