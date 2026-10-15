using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineSchoolApi.Users.Administrators;

namespace OnlineSchoolApi.Data.Configurations;

public class AdministratorConfiguration : IEntityTypeConfiguration<Administrator>
{
    public void Configure(EntityTypeBuilder<Administrator> builder)
    {
        builder.ToTable("administrator");

        builder.Property(x => x.EmployeeCode).HasColumnName("employee_code").HasMaxLength(50);
        builder.Property(x => x.Department).HasColumnName("department").HasMaxLength(100);
        builder.Property(x => x.JobTitle).HasColumnName("job_title").HasMaxLength(100);

        builder.HasIndex(x => x.EmployeeCode).IsUnique().HasDatabaseName("uk_administrator_employee_code");
    }
}
