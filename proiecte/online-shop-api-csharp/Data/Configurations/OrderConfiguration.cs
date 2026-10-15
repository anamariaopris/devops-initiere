using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopApi.Models;

namespace OnlineShopApi.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Ammount).HasColumnName("ammount").IsRequired();
        builder.Property(x => x.ShippingAdress).HasColumnName("shipping_adress").HasMaxLength(255).IsRequired();
        builder.Property(x => x.OrderAdress).HasColumnName("order_adress").HasMaxLength(255).IsRequired();
        builder.Property(x => x.OrderEmail).HasColumnName("order_email").HasMaxLength(255).IsRequired();
        builder.Property(x => x.OrderDate).HasColumnName("order_date").IsRequired();
        builder.Property(x => x.OrderStatus).HasColumnName("order_status").HasMaxLength(255).IsRequired();
        builder.Property(x => x.CustomerId).HasColumnName("customer_id");
        builder.ConfigureAuditColumns();

        builder
            .HasOne(x => x.Customer)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
