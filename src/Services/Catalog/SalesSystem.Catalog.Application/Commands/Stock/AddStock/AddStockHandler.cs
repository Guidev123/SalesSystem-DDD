using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Stock.AddStock
{
    public sealed class AddStockHandler : IRequestHandler<AddStockCommand, Response<AddStockResponse>>
    {
        public async Task<Response<AddStockResponse>> Handle(AddStockCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
