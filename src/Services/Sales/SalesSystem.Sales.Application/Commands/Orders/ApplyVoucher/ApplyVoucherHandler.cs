using MediatR;
using SalesSystem.Sales.Application.Events;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Commands.Orders.ApplyVoucher
{
    public sealed class ApplyVoucherHandler(IOrderRepository orderRepository,
                                            INotificator notificator)
                                          : IRequestHandler<ApplyVoucherCommand, Response<ApplyVoucherResponse>>
    {
        private readonly INotificator _notificator = notificator;
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<Response<ApplyVoucherResponse>> Handle(ApplyVoucherCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return Response<ApplyVoucherResponse>.Failure(request.GetErrorMessages());

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(request.CustomerId);

            if (order is null)
            {
                _notificator.HandleNotification(new("Order not foud."));
                return Response<ApplyVoucherResponse>.Failure(_notificator.GetNotifications(), code: 404);
            }

            var voucher = await _orderRepository.GetVoucherByCodeAsync(order.Code).ConfigureAwait(false);

            if(voucher is null)
            {
                _notificator.HandleNotification(new("Voucher not found."));
                return Response<ApplyVoucherResponse>.Failure(_notificator.GetNotifications(), code: 404);
            }

            var voucherApplyValidation = order.ApplyVoucher(voucher);
            if (!voucherApplyValidation.IsValid)
                return Response<ApplyVoucherResponse>.Failure([.. voucherApplyValidation.Errors.Select(x => x.ErrorMessage)]);

            return await PersistDataAsync(order, voucher, request).ConfigureAwait(false);
        }

        private async Task<Response<ApplyVoucherResponse>> PersistDataAsync(Order order, Voucher voucher, ApplyVoucherCommand request)
        {
            _orderRepository.Update(order);
            _orderRepository.UpdateVoucher(voucher);

            if (!await _orderRepository.UnitOfWork.CommitAsync().ConfigureAwait(false))
            {
                _notificator.HandleNotification(new("Fail to persist data."));
                return Response<ApplyVoucherResponse>.Failure(_notificator.GetNotifications());
            }

            order.AddEvent(new AppliedVoucherEvent(request.CustomerId, order.Id, voucher.Id));
            order.AddEvent(new UpdatedOrderEvent(order.Id, request.CustomerId, order.Price));

            return Response<ApplyVoucherResponse>.Success(default, code: 204);
        }
    }
}
