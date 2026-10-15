using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineSchoolApi.Enrolments;

namespace OnlineSchoolApi.Data.Configurations;

public class EnrolmentConfiguration : IEntityTypeConfiguration<Enrolment>
{
    public void Configure(EntityTypeBuilder<Enrolment> builder)
    {
        builder.ToTable("enrolment");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.StudentId).HasColumnName("student_id");
        builder.Property(x => x.CourseId).HasColumnName("course_id");

        builder.HasOne(x => x.Student)
            .WithMany(x => x.Enrolments)
            .HasForeignKey(x => x.StudentId)
            .HasConstraintName("fk_enrolment_student")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Course)
            .WithMany(x => x.Enrolments)
            .HasForeignKey(x => x.CourseId)
            .HasConstraintName("fk_enrolment_course")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
