using System.ComponentModel.DataAnnotations;

namespace OnlineSchoolApi.Books;

public record BookCreateRequest(
    [Required, StringLength(50, MinimumLength = 3)] string BookName,
    long? StudentId);

public record BookUpdateRequest(
    [StringLength(50, MinimumLength = 3)] string? BookName,
    long? StudentId);

public record BookResponse(int Id, string BookName, int? StudentId);

public static class BookMappings
{
    public static BookResponse ToResponse(this Book book) => new(book.Id, book.BookName, book.StudentId);
}
