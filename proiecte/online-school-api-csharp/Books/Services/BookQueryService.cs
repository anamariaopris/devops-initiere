using OnlineSchoolApi.Books.Dtos;
using OnlineSchoolApi.Books.Exceptions;
using OnlineSchoolApi.Books.Mappers;
using OnlineSchoolApi.Books.Repositories;

namespace OnlineSchoolApi.Books.Services;

public class BookQueryService(IBookRepository bookRepository) : IBookQueryService
{
    public async Task<List<BookResponse>> GetAllBooksAsync(CancellationToken cancellationToken = default)
        => (await bookRepository.GetAllAsync(cancellationToken)).Select(BookMapper.ToResponse).ToList();

    public async Task<BookResponse> GetBookByIdAsync(long bookId, CancellationToken cancellationToken = default)
    {
        var book = await bookRepository.GetByIdAsync(bookId, cancellationToken)
            ?? throw new BookNotFoundException(bookId);

        return BookMapper.ToResponse(book);
    }

    public async Task<List<BookResponse>> FindByStudentIdAsync(long studentId, CancellationToken cancellationToken = default)
        => (await bookRepository.GetByStudentIdAsync(studentId, cancellationToken)).Select(BookMapper.ToResponse).ToList();

    public async Task<List<BookResponse>> SearchBooksAsync(string? query, CancellationToken cancellationToken = default)
    {
        var normalized = query?.Trim() ?? string.Empty;
        if (normalized.Length == 0)
        {
            return [];
        }

        return (await bookRepository.SearchByNameAsync(normalized, cancellationToken)).Select(BookMapper.ToResponse).ToList();
    }
}
