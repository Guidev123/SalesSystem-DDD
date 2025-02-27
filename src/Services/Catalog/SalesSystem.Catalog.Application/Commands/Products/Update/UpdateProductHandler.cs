using MediatR;

namespace SalesSystem.Catalog.Application.Commands.Products.Update
{
    public sealed class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResponse>
    {
        public async Task<UpdateProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
