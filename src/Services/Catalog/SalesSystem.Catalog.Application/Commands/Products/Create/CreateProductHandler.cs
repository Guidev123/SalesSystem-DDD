using MidR.Interfaces;
using SalesSystem.Catalog.Application.Mappers;
using SalesSystem.Catalog.Application.Storage;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Products.Create
{
    public sealed class CreateProductHandler(IProductRepository productRepository,
                                             INotificator notificator,
                                             IBlobService blobService)
                                           : IRequestHandler<CreateProductCommand, Response<CreateProductResponse>>
    {
        public async Task<Response<CreateProductResponse>> ExecuteAsync(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<CreateProductResponse>.Failure(request.GetErrorMessages());

            var imageUrl = await ProcessImageAsync(request);
            if(string.IsNullOrEmpty(imageUrl))
                return Response<CreateProductResponse>.Failure(notificator.GetNotifications());

            var product = request.MapToEntity(imageUrl);

            productRepository.Create(product);

            if (!await productRepository.UnitOfWork.CommitAsync())
            {
                notificator.HandleNotification(new("Fail to persist data."));
                return Response<CreateProductResponse>.Failure(notificator.GetNotifications());
            }

            return Response<CreateProductResponse>.Success(new(product.Id));
        }

        private async Task<string?> ProcessImageAsync(CreateProductCommand command)
        {
            var imageStream = ConvertBase64ToStream(command.Image, out string contentType);
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
                return null;
            }
        }
    }
}