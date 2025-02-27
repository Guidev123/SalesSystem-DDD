using MediatR;

namespace SalesSystem.Catalog.Application.Commands.Stock.DebitStock
{
    public sealed class DebitStockHandler : IRequestHandler<DebitStockCommand, DebitStockResponse>
    {
        public async Task<DebitStockResponse> Handle(DebitStockCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
