using SalesSystem.Catalog.Application.Mappers;
using SalesSystem.Catalog.Application.Storage;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Categories.GetAll
{
    public sealed class GetAllCategoriesHandler(IProductRepository productRepository,
                                                INotificator notificator,
                                                ICacheService cache)
                                              : PagedQueryHandler<GetAllCategoriesQuery, GetAllCategoriesResponse>(notificator)
    {
        public override async Task<PagedResponse<GetAllCategoriesResponse>> ExecuteAsync(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"products_{request.PageNumber}_{request.PageSize}";
            var cacheCategory = await cache.GetAsync<PagedResponse<GetAllCategoriesResponse>>(cacheKey);
            if (cacheCategory is not null) return cacheCategory;

            var categories = await productRepository.GetAllCategoriesAsync();
            if (!categories.Any())
            {
                Notify("Categories not found");
                return PagedResponse<GetAllCategoriesResponse>.Failure(GetNotifications(), code: 404);
            }

            var pagedCategories = categories.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var categoriesResult = pagedCategories.Select(x => x.MapFromEntity());

            var response = PagedResponse<GetAllCategoriesResponse>.Success(new(categoriesResult), categories.Count(), request.PageNumber, request.PageSize);

            await cache.SetAsync(cacheKey, response);

            return response;
        }
    }
}