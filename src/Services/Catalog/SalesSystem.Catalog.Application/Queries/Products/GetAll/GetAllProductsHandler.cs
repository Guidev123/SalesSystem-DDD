using MediatR;
using SalesSystem.Catalog.Application.Mappers;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Products.GetAll
{
    public sealed class GetAllProductsHandler(IProductRepository productRepository,
                                              INotificator notificator)
                                            : IRequestHandler<GetAllProductsQuery, PagedResponse<GetAllProductsResponse>>
    {
        private readonly INotificator _notificator = notificator;
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<PagedResponse<GetAllProductsResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(request.PageNumber, request.PageSize);
            if (products is null)
            {
                _notificator.HandleNotification(new("Products not found"));
                return PagedResponse<GetAllProductsResponse>.Failure(_notificator.GetNotifications());
            }

            return PagedResponse<GetAllProductsResponse>.Success(new(products.Select(x => x.MapFromEntity())),
                products.Count(), request.PageNumber, request.PageSize);
        }
    }
}