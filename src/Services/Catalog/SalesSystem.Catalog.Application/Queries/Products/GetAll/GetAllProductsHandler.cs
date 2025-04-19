using SalesSystem.Catalog.Application.Mappers;
using SalesSystem.Catalog.Application.Storage;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Products.GetAll
{
    public sealed class GetAllProductsHandler(IProductRepository productRepository,
                                              INotificator notificator,
                                              ICacheService cache)
                                            : PagedQueryHandler<GetAllProductsQuery, GetAllProductsResponse>(notificator)
    {
        public override async Task<PagedResponse<GetAllProductsResponse>> ExecuteAsync(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"products_{request.PageNumber}_{request.PageSize}";
            var cacheProduct = await cache.GetAsync<PagedResponse<GetAllProductsResponse>>(cacheKey);
            if (cacheProduct is not null) return cacheProduct;

            var products = await productRepository.GetAllAsync();
            if (!products.Any())
            {
                Notify("Products not found");
                return PagedResponse<GetAllProductsResponse>.Failure(GetNotifications());
            }

            var pagedProducts = products.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var productsResult = pagedProducts.Select(x => x.MapFromEntity());

            var response = PagedResponse<GetAllProductsResponse>.Success(new(productsResult), products.Count(), request.PageNumber, request.PageSize);

            await cache.SetAsync(cacheKey, response);

            return response;
        }
    }
}