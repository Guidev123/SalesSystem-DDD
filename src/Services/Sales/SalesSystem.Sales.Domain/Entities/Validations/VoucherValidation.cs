using FluentValidation;

namespace SalesSystem.Sales.Domain.Entities.Validations
{
    internal sealed class VoucherValidation : AbstractValidator<Voucher>
    {
        public VoucherValidation()
        {
            RuleFor(x => x.ExpiresAt).Must(ValidateVoucherDate).WithMessage("Expired voucher.");

            RuleFor(x => x.IsActive).Equal(true).WithMessage("Inactive voucher.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Voucher sold out.");
        }

        private static bool ValidateVoucherDate(DateTime date) => date >= DateTime.Now;
    }
}
