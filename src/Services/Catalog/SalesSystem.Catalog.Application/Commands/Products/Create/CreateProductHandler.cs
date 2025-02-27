using MediatR;

namespace SalesSystem.Catalog.Application.Commands.Products.Create
{
    public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResponse>
    {
        public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
