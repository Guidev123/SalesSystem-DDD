using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Domain.Enums;

namespace Sales.UnitTests.Domain.Entities
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validate Voucher Valid Value")]
        [Trait("Sales Domain", "Voucher Tests")]
        public void Voucher_IsValidToApply_ValueVoucherShouldBeValid()
        {
            // Arrange
            var voucher = GetValidValueVoucher();

            // Act
            var result = voucher.IsValidToApply();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate Voucher Invalid Value")]
        [Trait("Sales Domain", "Voucher Tests")]
        public void Voucher_IsValidToApply_ValueVoucherShouldBeInvalid()
        {
            // Arrange
            var voucher = GetInvalidValueVoucher();

            // Act
            var result = voucher.IsValidToApply();

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Validate Voucher Valid Percentual")]
        [Trait("Sales Domain", "Voucher Tests")]
        public void Voucher_IsValidToApply_PercentualVoucherShouldBeValid()
        {
            // Arrange
            var voucher = GetValidPercentualVoucher();

            // Act
            var result = voucher.IsValidToApply();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate Voucher Invalid Percentual")]
        [Trait("Sales Domain", "Voucher Tests")]
        public void Voucher_IsValidToApply_PercentualVoucherShouldBeInvalid()
        {
            // Arrange
            var voucher = GetInvalidPercentualVoucher();

            // Act
            var result = voucher.IsValidToApply();

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Validate Voucher Expired")]
        [Trait("Sales Domain", "Voucher Tests")]
        public void Voucher_IsValidToApply_VoucherShouldBeExpired()
        {
            // Arrange
            var voucher = GetExpiredVoucher();

            // Act
            var result = voucher.IsValidToApply();

            // Assert
            Assert.False(result.IsValid);
        }

        private Voucher GetExpiredVoucher()
             => new(Guid.NewGuid().ToString("N"), null, 150, 100, EVoucherType.Value, DateTime.Now.AddDays(-1));

        private Voucher GetValidValueVoucher()
            => new(Guid.NewGuid().ToString("N"), null, 150, 100, EVoucherType.Value, DateTime.Now.AddDays(1));

        private Voucher GetValidPercentualVoucher()
             => new(Guid.NewGuid().ToString("N"), 150, null, 100, EVoucherType.Percentual, DateTime.Now.AddDays(1));
        private Voucher GetInvalidPercentualVoucher()
             => new(Guid.NewGuid().ToString("N"), null, 100, 100, EVoucherType.Percentual, DateTime.Now.AddDays(1));
        private Voucher GetInvalidValueVoucher()
             => new(Guid.NewGuid().ToString("N"), 100, null, 100, EVoucherType.Value, DateTime.Now.AddDays(1));
    }
}
