using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineSchoolApi.Users.Teachers;

namespace OnlineSchoolApi.Data.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("teacher");

        builder.Property(x => x.EmployeeCode).HasColumnName("employee_code").HasMaxLength(50);
        builder.Property(x => x.Specialization).HasColumnName("specialization").HasMaxLength(100);
        builder.Property(x => x.Title).HasColumnName("title").HasMaxLength(100);
        builder.Property(x => x.Bio).HasColumnName("bio").HasMaxLength(1000);

        builder.HasIndex(x => x.EmployeeCode).IsUnique().HasDatabaseName("uk_teacher_employee_code");
    }
}
