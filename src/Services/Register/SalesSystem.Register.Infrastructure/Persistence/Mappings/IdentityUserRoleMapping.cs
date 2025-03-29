using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SalesSystem.Register.Infrastructure.Persistence.Mappings;

public class IdentityUserRoleMapping : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.ToTable("UserRole");
        builder.HasKey(r => new { r.UserId, r.RoleId });
        builder.Property(x => x.UserId);
        builder.Property(x => x.RoleId);
    }
}