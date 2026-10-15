using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopApi.Models;

namespace OnlineShopApi.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Password).HasColumnName("password").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Enabled).HasColumnName("enabled").IsRequired();
        builder.ConfigureAuditColumns();

        builder.HasIndex(x => x.Email).IsUnique();

        builder
            .HasMany(x => x.Permissions)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "user_permissions",
                right => right.HasOne<Permission>().WithMany().HasForeignKey("permission_id").OnDelete(DeleteBehavior.Cascade),
                left => left.HasOne<User>().WithMany().HasForeignKey("user_id").OnDelete(DeleteBehavior.Cascade),
                join => join.HasKey("user_id", "permission_id"));
    }
}
