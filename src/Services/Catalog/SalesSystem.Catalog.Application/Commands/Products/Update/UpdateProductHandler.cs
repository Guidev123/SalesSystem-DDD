using MidR.Interfaces;
using SalesSystem.Catalog.Application.Storage;
using SalesSystem.Catalog.Domain.Entities;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Products.Update
{
    public sealed class UpdateProductHandler(IProductRepository productRepository,
                                             INotificator notificator,
                                             IBlobService blobService)
                                           : IRequestHandler<UpdateProductCommand, Response<UpdateProductResponse>>
    {
        public async Task<Response<UpdateProductResponse>> ExecuteAsync(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<UpdateProductResponse>.Failure(request.GetErrorMessages());

            var product = await productRepository.GetByIdAsync(request.Id);
            if (product is null)
            {
                notificator.HandleNotification(new("Product not found."));
                return Response<UpdateProductResponse>.Failure(notificator.GetNotifications(), code: 404);
            }

            var imageResult = await UpdateProduct(request, product);
            if(!imageResult)
                return Response<UpdateProductResponse>.Failure(notificator.GetNotifications());

            productRepository.Update(product);

            if (!await productRepository.UnitOfWork.CommitAsync())
            {
                notificator.HandleNotification(new("Fail to persist data."));
                return Response<UpdateProductResponse>.Failure(notificator.GetNotifications());
            }

            return Response<UpdateProductResponse>.Success(null, code: 204);
        }

        private async Task<bool> UpdateProduct(UpdateProductCommand command, Product product)
        {
            if (command.Image is not null)
            {
                var image = await UpdateImageAsync(command);
                if (string.IsNullOrEmpty(image)) return false;

                product.UpdateImage(image);
            }
            if (command.Price != 0 && command.Price != null) product.UpdatePrice((decimal)command.Price);
            if (command.Description is not null) product.UpdateDescription(command.Description);

            return true;
        }

        private async Task<string?> UpdateImageAsync(UpdateProductCommand command)
        {
            if (!IsBase64(command.Image!))
            {
                notificator.HandleNotification(new("Product image must be a valid Base64 string."));
                return null;
            }

            var image = await ProcessImageAsync(command);
            if (!string.IsNullOrEmpty(image)) return image;

            return null;
        }

        private async Task<string?> ProcessImageAsync(UpdateProductCommand command)
        {
            var imageStream = ConvertBase64ToStream(command.Image!, out string contentType);
            if (imageStream is null)
            {
                notificator.HandleNotification(new("Something has failed to process image."));
                return null;
            }

            var imageUrl = await blobService.UploadAsync(imageStream, contentType, default);
            if (string.IsNullOrEmpty(imageUrl))
            {
                notificator.HandleNotification(new("Something has failed to process image."));
                return null;
            }

            return imageUrl;
        }

        private MemoryStream? ConvertBase64ToStream(string base64, out string contentType)
        {
            try
            {
                var parts = base64.Split(',');
                var metadata = parts.Length > 1 ? parts[0] : "";
                var base64Data = parts.Length > 1 ? parts[1] : parts[0];

                contentType = metadata.Contains("image/") ? metadata.Split(';')[0].Split(':')[1] : "image/png";

                byte[] imageBytes = Convert.FromBase64String(base64Data);
                return new MemoryStream(imageBytes);
            }
            catch
            {
                contentType = "image/png";
                notificator.HandleNotification(new("The image is in invalid format."));
                return null;
            }
        }

        private bool IsBase64(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
                return false;

            Span<byte> buffer = new(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }
    }
}