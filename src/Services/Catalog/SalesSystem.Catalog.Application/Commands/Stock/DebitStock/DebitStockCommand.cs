using MediatR;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Stock.DebitStock
{
    public record DebitStockCommand(Guid Id, int Quantity) : IRequest<Response<DebitStockResponse>>;
}
