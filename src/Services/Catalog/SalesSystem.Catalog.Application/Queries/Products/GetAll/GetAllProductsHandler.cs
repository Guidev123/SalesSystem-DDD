using MediatR;

namespace SalesSystem.Catalog.Application.Queries.Products.GetAll
{
    public sealed class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, GetAllProductsResponse>
    {
        public async Task<GetAllProductsResponse> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
