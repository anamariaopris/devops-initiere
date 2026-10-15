using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineSchoolApi.Books;

namespace OnlineSchoolApi.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("book");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.BookName).HasColumnName("book_name").HasMaxLength(50).IsRequired();
        builder.Property(x => x.StudentId).HasColumnName("student_id");

        builder.HasOne(x => x.Student)
            .WithMany(x => x.Books)
            .HasForeignKey(x => x.StudentId)
            .HasConstraintName("fk_book_student")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
