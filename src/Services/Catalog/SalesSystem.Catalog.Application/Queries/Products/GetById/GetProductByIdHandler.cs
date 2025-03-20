using MediatR;
using SalesSystem.Catalog.Application.Mappers;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Products.GetById
{
    public sealed class GetProductByIdHandler(IProductRepository productRepository,
                                              INotificator notificator)
                                            : IRequestHandler<GetProductByIdQuery, Response<GetProductByIdResponse>>
    {
        private readonly INotificator _notificator = notificator;
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<Response<GetProductByIdResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product is null)
            {
                _notificator.HandleNotification(new("Product not found"));
                return PagedResponse<GetProductByIdResponse>.Failure(_notificator.GetNotifications());
            }

            return Response<GetProductByIdResponse>.Success(new(product.MapFromEntity()));
        }
    }
}