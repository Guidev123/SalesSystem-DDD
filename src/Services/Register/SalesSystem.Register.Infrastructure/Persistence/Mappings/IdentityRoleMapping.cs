using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SalesSystem.Register.Infrastructure.Persistence.Mappings;

public class IdentityRoleMapping : IEntityTypeConfiguration<IdentityRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<string>> builder)
    {
        builder.ToTable("Role");
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => r.NormalizedName).IsUnique();
        builder.Property(r => r.ConcurrencyStamp).HasColumnType("VARCHAR").HasMaxLength(160).IsConcurrencyToken();
        builder.Property(u => u.Name).HasColumnType("VARCHAR").HasMaxLength(160);
        builder.Property(u => u.NormalizedName).HasColumnType("VARCHAR").HasMaxLength(160);
    }
}