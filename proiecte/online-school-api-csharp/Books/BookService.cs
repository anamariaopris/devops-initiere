using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Data;
using OnlineSchoolApi.Errors;

namespace OnlineSchoolApi.Books;

public class BookService(AppDbContext db)
{
    public Task<List<BookResponse>> GetAllAsync(CancellationToken cancellationToken) =>
        ListAsync(db.Books, cancellationToken);

    public async Task<BookResponse> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var book = await db.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id, cancellationToken)
            ?? throw NotFound(id);
        return book.ToResponse();
    }

    public Task<List<BookResponse>> FindByStudentAsync(long studentId, CancellationToken cancellationToken) =>
        ListAsync(db.Books.Where(b => b.StudentId == studentId), cancellationToken);

    public async Task<List<BookResponse>> SearchAsync(string? query, CancellationToken cancellationToken)
    {
        var term = query?.Trim();
        if (string.IsNullOrEmpty(term))
        {
            return [];
        }
        return await ListAsync(db.Books.Where(b => b.BookName.Contains(term)), cancellationToken);
    }

    public async Task<BookResponse> CreateAsync(BookCreateRequest request, CancellationToken cancellationToken)
    {
        var book = new Book { BookName = request.BookName };
        if (request.StudentId is not null)
        {
            book.StudentId = await RequireStudentIdAsync(request.StudentId.Value, cancellationToken);
        }

        db.Books.Add(book);
        await db.SaveChangesAsync(cancellationToken);
        return book.ToResponse();
    }

    public async Task<BookResponse> UpdateAsync(long id, BookUpdateRequest request, CancellationToken cancellationToken)
    {
        var book = await FindAsync(id, cancellationToken);
        if (request is { BookName: null, StudentId: null })
        {
            throw BadRequestException.EmptyUpdate();
        }

        book.BookName = request.BookName ?? book.BookName;
        if (request.StudentId is not null)
        {
            book.StudentId = await RequireStudentIdAsync(request.StudentId.Value, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
        return book.ToResponse();
    }

    public async Task<BookResponse> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var book = await FindAsync(id, cancellationToken);
        db.Books.Remove(book);
        await db.SaveChangesAsync(cancellationToken);
        return book.ToResponse();
    }

    private static async Task<List<BookResponse>> ListAsync(IQueryable<Book> query, CancellationToken cancellationToken)
    {
        var books = await query.AsNoTracking().OrderBy(b => b.Id).ToListAsync(cancellationToken);
        return books.Select(b => b.ToResponse()).ToList();
    }

    private async Task<Book> FindAsync(long id, CancellationToken cancellationToken) =>
        await db.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken) ?? throw NotFound(id);

    private async Task<int> RequireStudentIdAsync(long studentId, CancellationToken cancellationToken)
    {
        var student = await db.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken)
            ?? throw new NotFoundException($"Student not found with id={studentId}");
        return student.Id;
    }

    private static NotFoundException NotFound(long id) => new($"Book not found with id={id}");
}
