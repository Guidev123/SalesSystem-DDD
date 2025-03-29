using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SalesSystem.Register.Infrastructure.Persistence.Mappings;

public class IdentityUserTokenMapping : IEntityTypeConfiguration<IdentityUserToken<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
    {
        builder.ToTable("UserToken");
        builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
        builder.Property(t => t.LoginProvider).HasColumnType("VARCHAR").HasMaxLength(160);
        builder.Property(t => t.Name).HasColumnType("VARCHAR").HasMaxLength(160);
        builder.Property(t => t.Value).HasColumnType("VARCHAR").HasMaxLength(200);
    }
}