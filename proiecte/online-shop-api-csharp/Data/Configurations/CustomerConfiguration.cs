using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopApi.Models;

namespace OnlineShopApi.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customer");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.FullName).HasColumnName("full_name").HasMaxLength(255).IsRequired();
        builder.Property(x => x.BillingAddress).HasColumnName("billing_address").HasMaxLength(255).IsRequired();
        builder.Property(x => x.DefaultShippingAddress).HasColumnName("default_shipping_address").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Country).HasColumnName("country").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(255).IsRequired();
        builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        builder.ConfigureAuditColumns();

        builder
            .HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<Customer>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
