using MediatR;
using SalesSystem.Catalog.Domain.Interfaces.Repositories;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Products.Create
{
    public sealed class CreateProductHandler(IProductRepository productRepository) : IRequestHandler<CreateProductCommand, Response<CreateProductResponse>>
    {
        private readonly IProductRepository _productRepository = productRepository;
        public Task<Response<CreateProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
