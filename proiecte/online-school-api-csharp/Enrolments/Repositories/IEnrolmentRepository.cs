using OnlineSchoolApi.Courses.Models;
using OnlineSchoolApi.Enrolments.Models;

namespace OnlineSchoolApi.Enrolments.Repositories;

public interface IEnrolmentRepository
{
    Task<List<Enrolment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Enrolment?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Enrolment?> GetTrackedByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<List<Enrolment>> GetByStudentIdAsync(long studentId, CancellationToken cancellationToken = default);
    Task<List<Enrolment>> GetByCourseIdAsync(long courseId, CancellationToken cancellationToken = default);
    Task<List<Enrolment>> GetByStudentIdAndCourseIdAsync(long studentId, long courseId, CancellationToken cancellationToken = default);
    Task<List<Course>> GetCoursesByStudentIdAsync(long studentId, CancellationToken cancellationToken = default);
    Task AddAsync(Enrolment enrolment, CancellationToken cancellationToken = default);
    void Remove(Enrolment enrolment);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
