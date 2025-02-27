using MediatR;

namespace SalesSystem.Catalog.Application.Commands.Stock.AddStock
{
    public sealed class AddStockHandler : IRequestHandler<AddStockCommand, AddStockResponse>
    {
        public async Task<AddStockResponse> Handle(AddStockCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
