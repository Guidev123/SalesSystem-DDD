using MediatR;

namespace SalesSystem.Catalog.Application.Commands.Stock.AddStock
{
    public record AddStockCommand(Guid Id, int Quantity) : IRequest<AddStockResponse>;
}
