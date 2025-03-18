using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Sales.Domain.Entities;

namespace SalesSystem.Sales.Infrastructure.Persistence.Mappings
{
    internal sealed class OrderItemMapping : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.UnitPrice).HasColumnType("MONEY").IsRequired();
            builder.Property(x => x.ProductName).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
        }
    }
}
