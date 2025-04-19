using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Catalog.Application.Commands.Stock.DebitStock
{
    public record DebitStockCommand(Guid Id, int Quantity) : Command<DebitStockResponse>
    {
    }
}