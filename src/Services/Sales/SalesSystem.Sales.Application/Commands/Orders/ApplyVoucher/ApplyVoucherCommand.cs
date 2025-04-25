using SalesSystem.SharedKernel.Abstractions;

namespace SalesSystem.Sales.Application.Commands.Orders.ApplyVoucher
{
    public record ApplyVoucherCommand : Command<ApplyVoucherResponse>
    {
        public ApplyVoucherCommand(string voucherCode)
        {
            VoucherCode = voucherCode;
        }

        public Guid CustomerId { get; private set; }
        public string VoucherCode { get; }

        public void SetCustomerId(Guid customerId)
        {
            CustomerId = customerId;
            AggregateId = customerId;
        }
    }
}