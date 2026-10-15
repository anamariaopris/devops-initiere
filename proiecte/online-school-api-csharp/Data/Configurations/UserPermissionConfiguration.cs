using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineSchoolApi.Users;

namespace OnlineSchoolApi.Data.Configurations;

public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.ToTable("user_permissions");
        builder.HasKey(x => new { x.UserId, x.Permission });

        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.Permission).HasColumnName("permission").HasConversion<string>().HasMaxLength(50).IsRequired();
    }
}
