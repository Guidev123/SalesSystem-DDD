using MidR.Interfaces;
using SalesSystem.Catalog.Application.Mappers;
using SalesSystem.Catalog.Application.Storage;
using SalesSystem.Catalog.Domain.Entities;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Products.GetAll
{
    public sealed class GetAllProductsHandler(IProductRepository productRepository,
                                              INotificator notificator,
                                              ICacheService cache)
                                            : IRequestHandler<GetAllProductsQuery, PagedResponse<GetAllProductsResponse>>
    {
        public async Task<PagedResponse<GetAllProductsResponse>> ExecuteAsync(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"products_{request.PageNumber}_{request.PageSize}";
            var cacheProduct = await cache.GetAsync<IEnumerable<Product>>(cacheKey);
            if (cacheProduct is not null)
                return PagedResponse<GetAllProductsResponse>.Success(new(cacheProduct.Select(x => x.MapFromEntity())),
                cacheProduct.Count(), request.PageNumber, request.PageSize);

            var products = await productRepository.GetAllAsync(request.PageNumber, request.PageSize);
            if (products is null)
            {
                notificator.HandleNotification(new("Products not found"));
                return PagedResponse<GetAllProductsResponse>.Failure(notificator.GetNotifications());
            }

            await cache.SetAsync(cacheKey, products);

            return PagedResponse<GetAllProductsResponse>.Success(new(products.Select(x => x.MapFromEntity())),
                products.Count(), request.PageNumber, request.PageSize);
        }
    }
}