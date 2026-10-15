using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineSchoolApi.Users;

namespace OnlineSchoolApi.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("app_user");
        builder.UseTptMappingStrategy();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.FirstName).HasColumnName("first_name").HasMaxLength(50).IsRequired();
        builder.Property(x => x.LastName).HasColumnName("last_name").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(50).IsRequired();
        builder.Property(x => x.PhoneNumber).HasColumnName("phone_number").HasMaxLength(30);
        builder.Property(x => x.PasswordHash).HasColumnName("password_hash").HasMaxLength(255);
        builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(x => x.LastLoginAt).HasColumnName("last_login_at");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        builder.HasIndex(x => x.Email).IsUnique().HasDatabaseName("uk_app_user_email");

        builder.HasMany(x => x.Permissions)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_user_permissions_user")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
