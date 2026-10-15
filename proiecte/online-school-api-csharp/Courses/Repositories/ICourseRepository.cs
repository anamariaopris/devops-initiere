using OnlineSchoolApi.Courses.Models;

namespace OnlineSchoolApi.Courses.Repositories;

public interface ICourseRepository
{
    Task<List<Course>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Course?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Course?> GetTrackedByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<List<Course>> GetByDepartmentContainingAsync(string department, CancellationToken cancellationToken = default);
    Task<List<Course>> SearchAsync(string term, CancellationToken cancellationToken = default);
    Task AddAsync(Course course, CancellationToken cancellationToken = default);
    void Remove(Course course);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
