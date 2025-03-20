using MediatR;
using SalesSystem.Catalog.Domain.Interfaces.Services;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Catalog.Application.Commands.Stock.DebitStock
{
    public sealed class DebitStockHandler(IStockService stockService,
                                          INotificator notificator)
                                        : IRequestHandler<DebitStockCommand, Response<DebitStockResponse>>
    {
        private readonly INotificator _notificator = notificator;
        private readonly IStockService _stockService = stockService;

        public async Task<Response<DebitStockResponse>> Handle(DebitStockCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<DebitStockResponse>.Failure(request.GetErrorMessages());

            if (!await _stockService.DebitStockAsync(request.Id, request.Quantity))
            {
                _notificator.HandleNotification(new("Fail to debit quantity."));
                return Response<DebitStockResponse>.Failure(_notificator.GetNotifications());
            }

            return Response<DebitStockResponse>.Success(null, code: 204);
        }
    }
}