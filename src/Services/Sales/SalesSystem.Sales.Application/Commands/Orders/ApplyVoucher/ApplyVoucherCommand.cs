using SalesSystem.SharedKernel.Messages;

namespace SalesSystem.Sales.Application.Commands.Orders.ApplyVoucher
{
    public record ApplyVoucherCommand : Command<ApplyVoucherResponse>
    {
        public ApplyVoucherCommand(Guid customerId, Guid orderId, string voucherCode)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            VoucherCode = voucherCode;
        }

        public Guid CustomerId { get; }
        public Guid OrderId { get; }
        public string VoucherCode { get; }

        public override bool IsValid()
        {
            SetValidationResult(new ApplyVoucherValidation().Validate(this));
            return ValidationResult.IsValid;
        }
    }
}