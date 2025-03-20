using FluentValidation;

namespace SalesSystem.Sales.Application.Commands.Orders.ApplyVoucher
{
    public sealed class ApplyVoucherValidation : AbstractValidator<ApplyVoucherCommand>
    {
        public ApplyVoucherValidation()
        {
            RuleFor(x => x.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("Customer Id cannot be empty.");

            RuleFor(x => x.OrderId)
                .NotEqual(Guid.Empty)
                .WithMessage("Order Id cannot be empty.");

            RuleFor(x => x.VoucherCode)
                .NotEmpty()
                .WithMessage("Voucher code cannot be empty.");
        }
    }
}