using SalesSystem.Catalog.Application.Mappers;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Queries.Products.GetById
{
    public sealed class GetProductByIdHandler(IProductRepository productRepository,
                                              INotificator notificator)
                                            : QueryHandler<GetProductByIdQuery, GetProductByIdResponse>(notificator)
    {
        public override async Task<Response<GetProductByIdResponse>> ExecuteAsync(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(request.Id);
            if (product is null)
            {
                Notify("Product not found");
                return PagedResponse<GetProductByIdResponse>.Failure(GetNotifications(), code: 404);
            }

            return Response<GetProductByIdResponse>.Success(new(product.MapFromEntity()));
        }
    }
}