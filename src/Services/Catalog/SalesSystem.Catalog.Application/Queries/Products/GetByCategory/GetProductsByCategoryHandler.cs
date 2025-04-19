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
            var products = await productRepository.GetByCategoryAsync(request.Code);
            if (!products.Any())
            {
                Notify("Products not found");
                return PagedResponse<GetProductsByCategoryResponse>.Failure(GetNotifications());
            }

            var pagedProducts = products.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            return PagedResponse<GetProductsByCategoryResponse>.Success(new(pagedProducts.Select(x => x.MapFromEntity())),
                products.Count(), request.PageNumber, request.PageSize);
        }
    }
}