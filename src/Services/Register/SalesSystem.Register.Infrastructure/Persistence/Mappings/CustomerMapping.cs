using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Register.Domain.Entities;

namespace SalesSystem.Register.Infrastructure.Persistence.Mappings
{
    public sealed class CustomerMapping : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(x => x.Id); 

            builder.OwnsOne(x => x.Email, email =>
            {
                email.Property(x => x.Address)
                .HasColumnName("Email").HasColumnType("VARCHAR(160)").IsRequired();
            });

            builder.OwnsOne(x => x.Document, document =>
            {
                document.Property(x => x.Number)
                .HasColumnName("Document").HasColumnType("VARCHAR(160)").IsRequired();
            });

            builder.Property(x => x.DeletedAt).IsRequired(false);

            builder.HasOne(x => x.Address).WithOne(x => x.Customer).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
