using OnlineSchoolApi.Books.Models;

namespace OnlineSchoolApi.Books.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Book?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Book?> GetTrackedByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<List<Book>> GetByStudentIdAsync(long studentId, CancellationToken cancellationToken = default);
    Task<List<Book>> SearchByNameAsync(string term, CancellationToken cancellationToken = default);
    Task AddAsync(Book book, CancellationToken cancellationToken = default);
    void Remove(Book book);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
