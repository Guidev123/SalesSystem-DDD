using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Sales.Domain.Entities;

namespace SalesSystem.Sales.Infrastructure.Persistence.Mappings
{
    internal sealed class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.OrderItems).WithOne(x => x.Order).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.Discount).HasColumnType("MONEY").IsRequired();
            builder.Property(x => x.Price).HasColumnType("MONEY").IsRequired();
            builder.Property(x => x.Code).IsRequired();
        }
    }
}
