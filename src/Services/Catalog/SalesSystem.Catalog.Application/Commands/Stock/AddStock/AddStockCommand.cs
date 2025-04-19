using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Catalog.Application.Commands.Stock.AddStock
{
    public record AddStockCommand(Guid Id, int Quantity) : Command<AddStockResponse>
    {
    }
}