using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineSchoolApi.Data.Seed;
using OnlineSchoolApi.Users.Students.Models;

namespace OnlineSchoolApi.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("student");

        builder.Property(x => x.StudentCode).HasColumnName("student_code").HasMaxLength(50);
        builder.Property(x => x.EnrollmentDate).HasColumnName("enrollment_date");
        builder.Property(x => x.DateOfBirth).HasColumnName("date_of_birth");
        builder.Property(x => x.GuardianName).HasColumnName("guardian_name").HasMaxLength(100);
        builder.Property(x => x.GuardianEmail).HasColumnName("guardian_email").HasMaxLength(100);

        builder.HasIndex(x => x.StudentCode).IsUnique().HasDatabaseName("uk_student_student_code");

        builder.HasData(InitialData.Students);
    }
}
