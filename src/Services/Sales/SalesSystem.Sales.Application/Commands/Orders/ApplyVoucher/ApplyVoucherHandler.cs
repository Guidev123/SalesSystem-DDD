using SalesSystem.Sales.Application.Events;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Domain.Repositories;
using SalesSystem.SharedKernel.Abstractions;
using SalesSystem.SharedKernel.Notifications;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.Sales.Application.Commands.Orders.ApplyVoucher
{
    public sealed class ApplyVoucherHandler(IOrderRepository orderRepository,
                                            INotificator notificator)
                                          : CommandHandler<ApplyVoucherCommand, ApplyVoucherResponse>(notificator)
    {
        public override async Task<Response<ApplyVoucherResponse>> ExecuteAsync(ApplyVoucherCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new ApplyVoucherValidation(), request))
                return Response<ApplyVoucherResponse>.Failure(GetNotifications());

            var order = await orderRepository.GetDraftOrderByCustomerIdAsync(request.CustomerId);

            if (order is null)
            {
                Notify("Order not foud.");
                return Response<ApplyVoucherResponse>.Failure(GetNotifications(), code: 404);
            }

            if (order.Price <= 0 || order.OrderItems.Count == 0)
            {
                Notify("Order price is zero or order is empty.");
                return Response<ApplyVoucherResponse>.Failure(GetNotifications());
            }

            var voucher = await orderRepository.GetVoucherByCodeAsync(request.VoucherCode).ConfigureAwait(false);

            if (voucher is null)
            {
                Notify("Voucher not found.");
                return Response<ApplyVoucherResponse>.Failure(GetNotifications(), code: 404);
            }

            var voucherApplyValidation = order.ApplyVoucher(voucher);
            if (!voucherApplyValidation.IsValid)
                return Response<ApplyVoucherResponse>.Failure([.. voucherApplyValidation.Errors.Select(x => x.ErrorMessage)]);

            return await PersistDataAsync(order, voucher, request).ConfigureAwait(false);
        }

        private async Task<Response<ApplyVoucherResponse>> PersistDataAsync(Order order, Voucher voucher, ApplyVoucherCommand request)
        {
            orderRepository.Update(order);
            orderRepository.UpdateVoucher(voucher);

            if (!await orderRepository.UnitOfWork.CommitAsync().ConfigureAwait(false))
            {
                Notify("Fail to persist data.");
                return Response<ApplyVoucherResponse>.Failure(GetNotifications());
            }

            order.AddEvent(new AppliedVoucherEvent(request.CustomerId, order.Id, voucher.Id));
            order.AddEvent(new UpdatedOrderEvent(order.Id, request.CustomerId, order.Price));

            return Response<ApplyVoucherResponse>.Success(default, code: 204);
        }
    }
}