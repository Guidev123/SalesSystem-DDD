using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Sales.Domain.Entities;

namespace SalesSystem.Sales.Infrastructure.Persistence.Mappings
{
    internal sealed class VoucherMapping : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable("Vouchers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code).IsRequired();
            builder.Property(x => x.Percentual).IsRequired(false).HasColumnType("MONEY");
            builder.Property(x => x.Value).IsRequired(false).HasColumnType("MONEY");
            builder.Property(x => x.Quantity).IsRequired();
            builder.HasMany(x => x.Orders).WithOne(x => x.Voucher).HasForeignKey(x => x.VoucherId);

            builder.HasQueryFilter(x => x.IsActive);
        }
    }
}
