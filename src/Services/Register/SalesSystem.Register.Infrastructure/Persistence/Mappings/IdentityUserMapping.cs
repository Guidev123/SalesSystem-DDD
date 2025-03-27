using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SalesSystem.Register.Infrastructure.Persistence.Mappings;

public class IdentityUserMapping : IEntityTypeConfiguration<IdentityUser>
{
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.NormalizedUserName).IsUnique();
        builder.HasIndex(u => u.NormalizedEmail).IsUnique();

        builder.Property(u => u.Email).HasColumnType("VARCHAR").HasMaxLength(160);
        builder.Property(u => u.NormalizedEmail).HasColumnType("VARCHAR").HasMaxLength(160);
        builder.Property(u => u.UserName).HasColumnType("VARCHAR").HasMaxLength(160);
        builder.Property(u => u.PasswordHash).HasColumnType("VARCHAR").HasMaxLength(255);
        builder.Property(u => u.SecurityStamp).HasColumnType("VARCHAR").HasMaxLength(255);
        builder.Property(u => u.ConcurrencyStamp).HasColumnType("VARCHAR").HasMaxLength(255);
        builder.Property(u => u.NormalizedUserName).HasColumnType("VARCHAR").HasMaxLength(160);
        builder.Property(u => u.PhoneNumber).HasColumnType("VARCHAR").HasMaxLength(160);
        builder.Property(u => u.ConcurrencyStamp).HasColumnType("VARCHAR").HasMaxLength(160).IsConcurrencyToken();

        builder.HasMany<IdentityUserClaim<string>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
        builder.HasMany<IdentityUserLogin<string>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
        builder.HasMany<IdentityUserToken<string>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
        builder.HasMany<IdentityUserRole<string>>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
    }
}
