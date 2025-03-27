using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SalesSystem.Register.Infrastructure.Persistence.Mappings;

public class IdentityUserLoginMapping : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        builder.ToTable("UserLogin");
        builder.HasKey(l => l.UserId);
        builder.Property(l => l.LoginProvider).HasColumnType("VARCHAR").HasMaxLength(160);
        builder.Property(l => l.ProviderKey).HasColumnType("VARCHAR").HasMaxLength(160);
        builder.Property(u => u.ProviderDisplayName).HasColumnType("VARCHAR").HasMaxLength(160);
    }
}
