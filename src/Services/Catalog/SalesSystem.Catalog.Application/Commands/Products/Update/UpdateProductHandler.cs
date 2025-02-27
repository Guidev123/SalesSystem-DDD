using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Products.Update
{
    public sealed class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Response<UpdateProductResponse>>
    {
        public async Task<Response<UpdateProductResponse>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
