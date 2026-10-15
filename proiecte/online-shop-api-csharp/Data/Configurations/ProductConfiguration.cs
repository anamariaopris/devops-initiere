using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopApi.Models;

namespace OnlineShopApi.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Sku).HasColumnName("sku").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Price).HasColumnName("price").HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.Weight).HasColumnName("weight").IsRequired();
        builder.Property(x => x.Descriptions).HasColumnName("descriptions").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Category).HasColumnName("category").HasMaxLength(255).IsRequired();
        builder.Property(x => x.CreateDate).HasColumnName("create_date").IsRequired();
        builder.Property(x => x.Stock).HasColumnName("stock").IsRequired();
        builder.ConfigureAuditColumns();
    }
}
