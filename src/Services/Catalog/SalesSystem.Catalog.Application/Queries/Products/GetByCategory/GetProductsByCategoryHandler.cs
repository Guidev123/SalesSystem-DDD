using MidR.Interfaces;
using SalesSystem.Catalog.Application.Mappers;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Products.GetByCategory
{
    public sealed class GetProductsByCategoryHandler(IProductRepository productRepository,
                                                    INotificator notificator)
                                                  : IRequestHandler<GetProductsByCategoryQuery, PagedResponse<GetProductsByCategoryResponse>>
    {
        private readonly INotificator _notificator = notificator;
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<PagedResponse<GetProductsByCategoryResponse>> ExecuteAsync(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetByCategoryAsync(request.Code);
            if (!products.Any())
            {
                _notificator.HandleNotification(new("Products not found"));
                return PagedResponse<GetProductsByCategoryResponse>.Failure(_notificator.GetNotifications());
            }

            var pagedProducts = products.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            return PagedResponse<GetProductsByCategoryResponse>.Success(new(pagedProducts.Select(x => x.MapFromEntity())),
                products.Count(), request.PageNumber, request.PageSize);
        }
    }
}