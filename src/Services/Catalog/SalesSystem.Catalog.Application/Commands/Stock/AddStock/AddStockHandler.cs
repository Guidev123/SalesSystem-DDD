using SalesSystem.Catalog.Domain.Interfaces.Services;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Stock.AddStock
{
    public sealed class AddStockHandler(IStockService stockService,
                                        INotificator notificator)
                                      : CommandHandler<AddStockCommand, AddStockResponse>(notificator)
    {
        public override async Task<Response<AddStockResponse>> ExecuteAsync(AddStockCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new AddStockValidation(), request))
                return Response<AddStockResponse>.Failure(GetNotifications());

            if (!await stockService.AddStockAsync(request.Id, request.Quantity))
            {
                Notify($"Fail to add {request.Quantity} products to stock.");
                return Response<AddStockResponse>.Failure(GetNotifications());
            }

            return Response<AddStockResponse>.Success(null, code: 204);
        }
    }
}