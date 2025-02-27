using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Products.Create
{
    public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, Response<CreateProductResponse>>
    {
        public Task<Response<CreateProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
