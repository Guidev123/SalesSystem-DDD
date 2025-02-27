using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Products.GetByCategory
{
    public sealed class GetProductByCategoryHandler : IRequestHandler<GetProductByCategoryQuery, Response<GetProductByCategoryResponse>>
    {
        public async Task<Response<GetProductByCategoryResponse>> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
