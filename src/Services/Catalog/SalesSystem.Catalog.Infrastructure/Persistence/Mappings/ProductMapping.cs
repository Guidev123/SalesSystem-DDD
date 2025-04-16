using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Catalog.Domain.Entities;

namespace SalesSystem.Catalog.Infrastructure.Persistence.Mappings
{
    public sealed class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.QuantityInStock).IsRequired().HasColumnType("INT");
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.Active).IsRequired();
            builder.Property(x => x.ImageUrl).HasMaxLength(255).HasColumnType("VARCHAR");
            builder.Property(x => x.Description).HasMaxLength(300).HasColumnType("VARCHAR");
            builder.Property(x => x.Price).HasColumnType("MONEY");
            builder.OwnsOne(x => x.Dimensions, tf =>
            {
                tf.Property(x => x.Height).HasColumnName("Height").HasColumnType("int").IsRequired();
                tf.Property(x => x.Depth).HasColumnName("Depth").HasColumnType("int").IsRequired();
                tf.Property(x => x.Width).HasColumnName("Width").HasColumnType("int").IsRequired();
            });

            builder.HasOne(x => x.Category)
                       .WithMany()
                       .HasForeignKey(x => x.CategoryId)
                       .IsRequired();
        }
    }
}