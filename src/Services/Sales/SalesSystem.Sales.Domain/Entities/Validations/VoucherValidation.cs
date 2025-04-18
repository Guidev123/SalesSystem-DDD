using FluentValidation;
using SalesSystem.Sales.Domain.Enums;

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

            When(v => v.Type == EVoucherType.Value, () =>
            {
                RuleFor(x => x.Value)
                    .NotNull()
                    .GreaterThan(0)
                    .WithMessage("Value voucher must have a value.");

                RuleFor(x => x.Percentual)
                    .Null()
                    .WithMessage("Percentual voucher must be null.");
            });

            When(v => v.Type == EVoucherType.Percentual, () =>
            {
                RuleFor(x => x.Percentual)
                    .NotNull()
                    .GreaterThan(0)
                    .WithMessage("Percentual voucher must have a percentual.");

                RuleFor(x => x.Value)
                    .Null()
                    .WithMessage("Value voucher must be null.");
            });
        }

        private static bool ValidateVoucherDate(DateTime date) => date >= DateTime.Now;
    }
}