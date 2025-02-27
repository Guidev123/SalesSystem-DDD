using MediatR;

namespace SalesSystem.Catalog.Application.Commands.Stock.DebitStock
{
    public record DebitStockCommand(Guid Id, int Quantity) : IRequest<DebitStockResponse>;
}
