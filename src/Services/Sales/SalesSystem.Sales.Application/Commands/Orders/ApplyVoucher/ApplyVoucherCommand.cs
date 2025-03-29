using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Sales.Application.Commands.Orders.ApplyVoucher
{
    public record ApplyVoucherCommand : Command<ApplyVoucherResponse>
    {
        public ApplyVoucherCommand(Guid orderId, string voucherCode)
        {
            OrderId = orderId;
            VoucherCode = voucherCode;
        }

        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; }
        public string VoucherCode { get; }

        public void SetCustomerId(Guid customerId)
        {
            CustomerId = customerId;
            AggregateId = customerId;
        }

        public override bool IsValid()
        {
            SetValidationResult(new ApplyVoucherValidation().Validate(this));
            return ValidationResult!.IsValid;
        }
    }
}