using FluentValidation.Results;
using SalesSystem.Sales.Domain.Entities.Validations;
using SalesSystem.Sales.Domain.Enums;
using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Sales.Domain.Entities
{
    public class Voucher : Entity
    {
        public Voucher(string code, decimal? percentual, decimal? value, int quantity, EVoucherType type, DateTime expiresAt)
        {
            Code = code;
            Percentual = percentual;
            Value = value;
            Quantity = quantity;
            Type = type;
            CreatedAt = DateTime.Now;
            ExpiresAt = expiresAt;
            IsActive = true;
            Used = false;
            Validate();
        }

        protected Voucher()
        { }

        public string Code { get; private set; } = string.Empty;
        public decimal? Percentual { get; private set; }
        public decimal? Value { get; private set; }
        public int Quantity { get; private set; }
        public EVoucherType Type { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool IsActive { get; private set; }
        public bool Used { get; private set; }
        public ICollection<Order> Orders { get; private set; } = [];

        public override void Validate()
        {
            AssertionConcern.EnsureGreaterThan(Quantity, 0, "The quantity must be greater than zero.");
            AssertionConcern.EnsureNotEmpty(Code, "The voucher code cannot be empty.");
            AssertionConcern.EnsureGreaterThan(Quantity, 0, "The voucher quantity must be greater than zero.");
            AssertionConcern.EnsureNotNull(Type, "The voucher type cannot be null.");

            if (Percentual.HasValue && Value.HasValue)
                throw new DomainException("A voucher cannot have both percentual and value discounts.");

            if (!Percentual.HasValue && !Value.HasValue)
                throw new DomainException("A voucher must have either a percentual or a value discount.");
        }

        public ValidationResult IsValidToApply()
            => new VoucherValidation().Validate(this);

        internal void DebitVoucherQuantity()
        {
            Used = true;
            Quantity -= 1;
        }
    }
}