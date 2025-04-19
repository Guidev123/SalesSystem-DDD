using SalesSystem.Catalog.Domain.Interfaces.Services;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Stock.DebitStock
{
    public sealed class DebitStockHandler(IStockService stockService,
                                          INotificator notificator)
                                        : CommandHandler<DebitStockCommand, DebitStockResponse>(notificator)
    {
        public override async Task<Response<DebitStockResponse>> ExecuteAsync(DebitStockCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new DebitStockValidation(), request))
                return Response<DebitStockResponse>.Failure(GetNotifications());

            if (!await stockService.DebitStockAsync(request.Id, request.Quantity))
            {
                Notify("Fail to debit quantity.");
                return Response<DebitStockResponse>.Failure(GetNotifications());
            }

            return Response<DebitStockResponse>.Success(null, code: 204);
        }
    }
}