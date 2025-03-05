using MediatR;
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

        public async Task<PagedResponse<GetProductsByCategoryResponse>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetByCategoryAsync(request.PageNumber, request.PageSize, request.Code);
            if (products is null)
            {
                _notificator.HandleNotification(new("Products not found"));
                return PagedResponse<GetProductsByCategoryResponse>.Failure(_notificator.GetNotifications());
            }

            return PagedResponse<GetProductsByCategoryResponse>.Success(new(products.Select(x => x.MapFromEntity())),
                products.Count(), request.PageNumber, request.PageSize);
        }
    }
}
