using OnlineSchoolApi.Books.Dtos;

namespace OnlineSchoolApi.Books.Services;

public interface IBookQueryService
{
    Task<List<BookResponse>> GetAllBooksAsync(CancellationToken cancellationToken = default);
    Task<BookResponse> GetBookByIdAsync(long bookId, CancellationToken cancellationToken = default);
    Task<List<BookResponse>> FindByStudentIdAsync(long studentId, CancellationToken cancellationToken = default);
    Task<List<BookResponse>> SearchBooksAsync(string? query, CancellationToken cancellationToken = default);
}
