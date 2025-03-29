using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Payments.Domain.Entities;

namespace SalesSystem.Payments.Infrastructure.Persistence.Mappings
{
    public sealed class TransactionMapping : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Total).IsRequired(true).HasColumnType("MONEY");
            builder.Property(p => p.ExternalReference).IsRequired(false);
        }
    }
}