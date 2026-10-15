using OnlineSchoolApi.Users.Teachers.Models;

namespace OnlineSchoolApi.Users.Teachers.Repositories;

public interface ITeacherRepository
{
    Task<List<Teacher>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Teacher?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Teacher?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<List<Teacher>> SearchAsync(string term, CancellationToken cancellationToken = default);
}
