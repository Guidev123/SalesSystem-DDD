using MediatR;

namespace SalesSystem.Catalog.Application.Queries.Products.GetByCategory
{
    public sealed class GetProductByCategoryHandler : IRequestHandler<GetProductByCategoryQuery, GetProductByCategoryResponse>
    {
        public async Task<GetProductByCategoryResponse> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
