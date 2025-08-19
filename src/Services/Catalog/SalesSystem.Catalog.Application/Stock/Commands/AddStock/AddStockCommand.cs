using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Catalog.Application.Stock.Commands.AddStock
{
    public record AddStockCommand(Guid Id, int Quantity) : Command<AddStockResponse>
    {
    }
}