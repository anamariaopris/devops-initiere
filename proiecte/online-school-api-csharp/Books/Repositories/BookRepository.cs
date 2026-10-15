using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Books.Models;
using OnlineSchoolApi.Common.Extensions;
using OnlineSchoolApi.Data;

namespace OnlineSchoolApi.Books.Repositories;

public class BookRepository(AppDbContext dbContext) : IBookRepository
{
    public Task<List<Book>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Books.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task<Book?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var key = id.ToEntityId();
        return key is null
            ? null
            : await dbContext.Books.AsNoTracking().FirstOrDefaultAsync(x => x.Id == key.Value, cancellationToken);
    }

    public async Task<Book?> GetTrackedByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var key = id.ToEntityId();
        return key is null
            ? null
            : await dbContext.Books.FirstOrDefaultAsync(x => x.Id == key.Value, cancellationToken);
    }

    public async Task<List<Book>> GetByStudentIdAsync(long studentId, CancellationToken cancellationToken = default)
    {
        var key = studentId.ToEntityId();
        if (key is null)
        {
            return [];
        }

        return await dbContext.Books.AsNoTracking()
            .Where(x => x.StudentId == key.Value)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Book>> SearchByNameAsync(string term, CancellationToken cancellationToken = default)
    {
        var normalized = term.ToLower();
        return dbContext.Books.AsNoTracking()
            .Where(x => x.BookName.ToLower().Contains(normalized))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        await dbContext.Books.AddAsync(book, cancellationToken);
    }

    public void Remove(Book book)
    {
        dbContext.Books.Remove(book);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
