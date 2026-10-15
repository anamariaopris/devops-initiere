using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OneToManyApi.Data.Seed;
using OneToManyApi.Models;

namespace OneToManyApi.Data.Configurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("car");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Brand).HasColumnName("brand").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Model).HasColumnName("model").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Year).HasColumnName("year").IsRequired();
        builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Cars)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(InitialData.Cars);
    }
}
