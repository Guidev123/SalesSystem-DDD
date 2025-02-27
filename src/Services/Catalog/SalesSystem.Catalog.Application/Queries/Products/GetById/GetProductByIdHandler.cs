using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Products.GetById
{
    public sealed class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Response<GetProductByIdResponse>>
    {
        public async Task<Response<GetProductByIdResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
