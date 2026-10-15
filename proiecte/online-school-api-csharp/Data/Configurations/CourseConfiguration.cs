using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineSchoolApi.Courses;

namespace OnlineSchoolApi.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("course");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.CourseName).HasColumnName("course_name").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Department).HasColumnName("department").HasMaxLength(255).IsRequired();
    }
}
