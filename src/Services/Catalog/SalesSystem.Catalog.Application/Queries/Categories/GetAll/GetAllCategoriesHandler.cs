using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Categories.GetAll
{
    public sealed class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, PagedResponse<GetAllCategoriesResponse>>
    {
        public async Task<PagedResponse<GetAllCategoriesResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
