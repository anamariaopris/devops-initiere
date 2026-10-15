using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopApi.Models;

namespace OnlineShopApi.Data.Configurations;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.ToTable("order_details");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Price).HasColumnName("price").HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.Sku).HasColumnName("sku").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Quantity).HasColumnName("quantity").IsRequired();
        builder.Property(x => x.OrderId).HasColumnName("order_id");
        builder.Property(x => x.ProductId).HasColumnName("product_id");
        builder.ConfigureAuditColumns();

        builder
            .HasOne(x => x.Order)
            .WithMany(x => x.OrderDetails)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.OrderDetails)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
