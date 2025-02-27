using MediatR;

namespace SalesSystem.Catalog.Application.Queries.Categories.GetAll
{
    public sealed class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, GetAllCategoriesResponse>
    {
        public async Task<GetAllCategoriesResponse> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
