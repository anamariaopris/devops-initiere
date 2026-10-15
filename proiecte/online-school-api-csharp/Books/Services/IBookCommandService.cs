using OnlineSchoolApi.Books.Dtos;

namespace OnlineSchoolApi.Books.Services;

public interface IBookCommandService
{
    Task<BookResponse> CreateBookAsync(BookCreateRequest? request, CancellationToken cancellationToken = default);
    Task<BookResponse> UpdateBookAsync(long bookId, BookUpdateRequest? request, CancellationToken cancellationToken = default);
    Task<BookResponse> DeleteBookAsync(long bookId, CancellationToken cancellationToken = default);
}
