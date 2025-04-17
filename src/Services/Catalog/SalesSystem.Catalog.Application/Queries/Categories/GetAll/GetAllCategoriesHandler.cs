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
            var cacheCategory = await cache.GetAsync<PagedResponse<GetAllCategoriesResponse>>(cacheKey);
            if (cacheCategory is not null) return cacheCategory;

            var categories = await productRepository.GetAllCategoriesAsync();
            if (!categories.Any())
            {
                notification.HandleNotification(new("Categories not found"));
                return PagedResponse<GetAllCategoriesResponse>.Failure(notification.GetNotifications(), code: 404);
            }

            var pagedCategories = categories.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var categoriesResult = pagedCategories.Select(x => x.MapFromEntity());

            var response = PagedResponse<GetAllCategoriesResponse>.Success(new(categoriesResult), categories.Count(), request.PageNumber, request.PageSize);

            await cache.SetAsync(cacheKey, response);

            return response;
        }
    }
}