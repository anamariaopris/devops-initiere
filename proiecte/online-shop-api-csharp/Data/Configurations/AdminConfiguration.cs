using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopApi.Models;

namespace OnlineShopApi.Data.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("admin");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.DisplayName).HasColumnName("display_name").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Department).HasColumnName("department").HasMaxLength(255);
        builder.Property(x => x.Notes).HasColumnName("notes").HasMaxLength(1000);
        builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        builder.ConfigureAuditColumns();

        builder
            .HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<Admin>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
