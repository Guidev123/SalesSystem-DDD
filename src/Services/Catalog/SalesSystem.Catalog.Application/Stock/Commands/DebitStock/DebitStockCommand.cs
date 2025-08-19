using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Catalog.Application.Stock.Commands.DebitStock
{
    public record DebitStockCommand(Guid Id, int Quantity) : Command<DebitStockResponse>
    {
    }
}