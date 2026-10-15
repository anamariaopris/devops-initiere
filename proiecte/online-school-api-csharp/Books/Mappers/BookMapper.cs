using OnlineSchoolApi.Books.Dtos;
using OnlineSchoolApi.Books.Models;

namespace OnlineSchoolApi.Books.Mappers;

public static class BookMapper
{
    public static Book ToEntity(BookCreateRequest request) => new()
    {
        BookName = request.BookName ?? string.Empty
    };

    public static BookResponse ToResponse(Book book) => new(book.Id, book.BookName, book.StudentId);
}
