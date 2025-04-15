using MidR.Interfaces;
using SalesSystem.Catalog.Application.Mappers;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Categories.GetAll
{
    public sealed class GetAllCategoriesHandler(IProductRepository productRepository,
                                                INotificator notification)
                                              : IRequestHandler<GetAllCategoriesQuery, Response<GetAllCategoriesResponse>>
    {
        private readonly INotificator _notification = notification;
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<Response<GetAllCategoriesResponse>> ExecuteAsync(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _productRepository.GetAllCategoriesAsync();
            if (categories is null)
            {
                _notification.HandleNotification(new("Categories not found"));
                return Response<GetAllCategoriesResponse>.Failure(_notification.GetNotifications(), code: 404);
            }

            return Response<GetAllCategoriesResponse>.Success(new(categories.Select(x => x.MapFromEntity())));
        }
    }
}