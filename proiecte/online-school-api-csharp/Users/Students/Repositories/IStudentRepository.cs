using OnlineSchoolApi.Users.Students.Models;

namespace OnlineSchoolApi.Users.Students.Repositories;

public interface IStudentRepository
{
    Task<List<Student>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Student?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Student?> GetTrackedByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Student?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<List<Student>> SearchAsync(string term, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string lowerCaseEmail, CancellationToken cancellationToken = default);
    Task AddAsync(Student student, CancellationToken cancellationToken = default);
    void Remove(Student student);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
