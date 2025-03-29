using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Payments.Domain.Entities;

namespace SalesSystem.Payments.Infrastructure.Persistence.Mappings
{
    public sealed class PaymentMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Amount).IsRequired(true).HasColumnType("MONEY");
            builder.HasOne(x => x.Transaction).WithOne(x => x.Payment);
        }
    }
}