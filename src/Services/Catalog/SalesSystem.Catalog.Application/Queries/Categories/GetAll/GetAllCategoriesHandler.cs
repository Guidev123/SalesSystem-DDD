using MidR.Interfaces;
using SalesSystem.Catalog.Application.DTOs;
using SalesSystem.Catalog.Application.Mappers;
using SalesSystem.Catalog.Application.Storage;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Categories.GetAll
{
    public sealed class GetAllCategoriesHandler(IProductRepository productRepository,
                                                INotificator notification,
                                                ICacheService cache)
                                              : IRequestHandler<GetAllCategoriesQuery, PagedResponse<GetAllCategoriesResponse>>
    {
        public async Task<PagedResponse<GetAllCategoriesResponse>> ExecuteAsync(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"products_{request.PageNumber}_{request.PageSize}";
            var cacheCategory = await cache.GetAsync<IEnumerable<CategoryDto>>(cacheKey);
            if (cacheCategory is not null)
                return PagedResponse<GetAllCategoriesResponse>.Success(new(cacheCategory), cacheCategory.Count(), request.PageNumber, request.PageSize);

            var categories = await productRepository.GetAllCategoriesAsync();
            if (categories is null)
            {
                notification.HandleNotification(new("Categories not found"));
                return PagedResponse<GetAllCategoriesResponse>.Failure(notification.GetNotifications(), code: 404);
            }

            var categoriesResult = categories.Select(x => x.MapFromEntity());

            await cache.SetAsync(cacheKey, categoriesResult);

            return PagedResponse<GetAllCategoriesResponse>.Success(new(categoriesResult), categoriesResult.Count(), request.PageNumber, request.PageSize);
        }
    }
}