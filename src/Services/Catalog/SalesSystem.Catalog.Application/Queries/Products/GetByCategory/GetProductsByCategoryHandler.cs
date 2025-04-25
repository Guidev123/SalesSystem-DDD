using SalesSystem.Catalog.Application.Mappers;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Products.GetByCategory
{
    public sealed class GetProductsByCategoryHandler(IProductRepository productRepository,
                                                    INotificator notificator)
                                                  : PagedQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResponse>(notificator)
    {
        public override async Task<PagedResponse<GetProductsByCategoryResponse>> ExecuteAsync(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var (products, totalCount) = await productRepository.GetByCategoryAsync(request.PageNumber, request.PageSize, request.Code);
            if (!products.Any() || totalCount <= 0)
            {
                Notify("Products not found");
                return PagedResponse<GetProductsByCategoryResponse>.Failure(GetNotifications());
            }

            return PagedResponse<GetProductsByCategoryResponse>.Success(new(products.Select(x => x.MapFromEntity())),
                totalCount, request.PageNumber, request.PageSize);
        }
    }
}