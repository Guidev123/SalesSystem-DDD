using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Stock.AddStock
{
    public record AddStockCommand(Guid Id, int Quantity) : IRequest<Response<AddStockResponse>>;
}
