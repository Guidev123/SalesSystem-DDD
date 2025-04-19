using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SalesSystem.Catalog.Application.Storage;

namespace SalesSystem.Catalog.Infrastructure.Storage
{
    internal sealed class BlobService(BlobServiceClient blob) : IBlobService
    {
        private const string CONTAINER_NAME = "images";

        public async Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default)
        {
            var containerClient = blob.GetBlobContainerClient(CONTAINER_NAME);

            var blobClient = containerClient.GetBlobClient(fileId.ToString());

            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }

        public async Task<string> UploadAsync(Stream stream, string contentType, CancellationToken cancellationToken = default)
        {
            var containerClient = blob.GetBlobContainerClient(CONTAINER_NAME);

            var fileId = Guid.NewGuid();
            var blobClient = containerClient.GetBlobClient(fileId.ToString());

            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);

            return blobClient.Uri.ToString();
        }
    }
}