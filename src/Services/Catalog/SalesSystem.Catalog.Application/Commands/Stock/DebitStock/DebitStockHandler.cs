using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Stock.DebitStock
{
    public sealed class DebitStockHandler : IRequestHandler<DebitStockCommand, Response<DebitStockResponse>>
    {
        public async Task<Response<DebitStockResponse>> Handle(DebitStockCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
