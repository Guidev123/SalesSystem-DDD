using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesSystem.Registers.Domain.Entities;

namespace SalesSystem.Registers.Infrastructure.Persistence.Mappings
{
    public sealed class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);
            builder.ToTable("Addresses");

            builder.Property(a => a.Street)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(a => a.Number)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(a => a.ZipCode)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(a => a.AdditionalInfo)
                .HasColumnType("varchar(250)");

            builder.Property(a => a.Neighborhood)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(a => a.City)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(a => a.State)
                .IsRequired()
                .HasColumnType("varchar(50)");
        }
    }
}