using MidR.Interfaces;
using SalesSystem.Catalog.Domain.Entities;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Products.Update
{
    public sealed class UpdateProductHandler(IProductRepository productRepository,
                                             INotificator notificator)
                                           : IRequestHandler<UpdateProductCommand, Response<UpdateProductResponse>>
    {
        private readonly INotificator _notificator = notificator;
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<Response<UpdateProductResponse>> ExecuteAsync(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<UpdateProductResponse>.Failure(request.GetErrorMessages());

            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product is null)
            {
                _notificator.HandleNotification(new("Product not found."));
                return Response<UpdateProductResponse>.Failure(_notificator.GetNotifications(), code: 404);
            }

            UpdateProduct(request, product);

            _productRepository.Update(product);

            if (!await _productRepository.UnitOfWork.CommitAsync())
            {
                _notificator.HandleNotification(new("Fail to persist data."));
                return Response<UpdateProductResponse>.Failure(_notificator.GetNotifications());
            }

            return Response<UpdateProductResponse>.Success(null, code: 204);
        }

        private static void UpdateProduct(UpdateProductCommand command, Product product)
        {
            if (command.Image is not null) product.UpdateImage(command.Image);
            if (command.Price != 0 && command.Price != null) product.UpdatePrice((decimal)command.Price);
            if (command.Description is not null) product.UpdateDescription(command.Description);
        }
    }
}