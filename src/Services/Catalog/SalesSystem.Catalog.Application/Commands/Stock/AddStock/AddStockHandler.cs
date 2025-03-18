using MediatR;
using SalesSystem.Catalog.Application.Commands.Products.Update;
using SalesSystem.Catalog.Domain.Interfaces.Services;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Stock.AddStock
{
    public sealed class AddStockHandler(IStockService stockService,
                                        INotificator notificator)
                                      : IRequestHandler<AddStockCommand, Response<AddStockResponse>>
    {
        private readonly INotificator _notificator = notificator;
        private readonly IStockService _stockService = stockService;

        public async Task<Response<AddStockResponse>> Handle(AddStockCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<AddStockResponse>.Failure(request.GetErrorMessages());

            if (!await _stockService.AddStockAsync(request.Id, request.Quantity))
            {
                _notificator.HandleNotification(new($"Fail to add {request.Quantity} products to stock."));
                return Response<AddStockResponse>.Failure(_notificator.GetNotifications());
            }

            return Response<AddStockResponse>.Success(null, code: 204);
        }
    }
}
