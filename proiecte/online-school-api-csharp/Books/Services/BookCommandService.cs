using OnlineSchoolApi.Books.Dtos;
using OnlineSchoolApi.Books.Exceptions;
using OnlineSchoolApi.Books.Mappers;
using OnlineSchoolApi.Books.Repositories;
using OnlineSchoolApi.Common.Exceptions;
using OnlineSchoolApi.Users.Students.Exceptions;
using OnlineSchoolApi.Users.Students.Repositories;

namespace OnlineSchoolApi.Books.Services;

public class BookCommandService(IBookRepository bookRepository, IStudentRepository studentRepository) : IBookCommandService
{
    public async Task<BookResponse> CreateBookAsync(BookCreateRequest? request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new RequestBodyMissingException();
        }

        var book = BookMapper.ToEntity(request);
        if (request.StudentId is not null)
        {
            book.StudentId = await RequireStudentIdAsync(request.StudentId.Value, cancellationToken);
        }

        await bookRepository.AddAsync(book, cancellationToken);
        await bookRepository.SaveChangesAsync(cancellationToken);
        return BookMapper.ToResponse(book);
    }

    public async Task<BookResponse> UpdateBookAsync(long bookId, BookUpdateRequest? request, CancellationToken cancellationToken = default)
    {
        var book = await bookRepository.GetTrackedByIdAsync(bookId, cancellationToken)
            ?? throw new BookNotFoundException(bookId);

        if (request is null)
        {
            throw new RequestBodyMissingException();
        }

        if (request.BookName is null && request.StudentId is null)
        {
            throw new EmptyUpdateRequestException();
        }

        if (request.BookName is not null)
        {
            book.BookName = request.BookName;
        }

        if (request.StudentId is not null)
        {
            book.StudentId = await RequireStudentIdAsync(request.StudentId.Value, cancellationToken);
        }

        await bookRepository.SaveChangesAsync(cancellationToken);
        return BookMapper.ToResponse(book);
    }

    public async Task<BookResponse> DeleteBookAsync(long bookId, CancellationToken cancellationToken = default)
    {
        var book = await bookRepository.GetTrackedByIdAsync(bookId, cancellationToken)
            ?? throw new BookNotFoundException(bookId);

        var response = BookMapper.ToResponse(book);
        bookRepository.Remove(book);
        await bookRepository.SaveChangesAsync(cancellationToken);
        return response;
    }

    private async Task<int> RequireStudentIdAsync(long studentId, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetByIdAsync(studentId, cancellationToken)
            ?? throw new StudentNotFoundException(studentId);

        return student.Id;
    }
}
