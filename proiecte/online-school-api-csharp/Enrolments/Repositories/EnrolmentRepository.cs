using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Common.Extensions;
using OnlineSchoolApi.Courses.Models;
using OnlineSchoolApi.Data;
using OnlineSchoolApi.Enrolments.Models;

namespace OnlineSchoolApi.Enrolments.Repositories;

public class EnrolmentRepository(AppDbContext dbContext) : IEnrolmentRepository
{
    public Task<List<Enrolment>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Enrolments.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<Enrolment?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => dbContext.Enrolments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Enrolment?> GetTrackedByIdAsync(long id, CancellationToken cancellationToken = default)
        => dbContext.Enrolments.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<List<Enrolment>> GetByStudentIdAsync(long studentId, CancellationToken cancellationToken = default)
    {
        var key = studentId.ToEntityId();
        if (key is null)
        {
            return [];
        }

        return await dbContext.Enrolments.AsNoTracking()
            .Where(x => x.StudentId == key.Value)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Enrolment>> GetByCourseIdAsync(long courseId, CancellationToken cancellationToken = default)
    {
        var key = courseId.ToEntityId();
        if (key is null)
        {
            return [];
        }

        return await dbContext.Enrolments.AsNoTracking()
            .Where(x => x.CourseId == key.Value)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Enrolment>> GetByStudentIdAndCourseIdAsync(long studentId, long courseId, CancellationToken cancellationToken = default)
    {
        var studentKey = studentId.ToEntityId();
        var courseKey = courseId.ToEntityId();
        if (studentKey is null || courseKey is null)
        {
            return [];
        }

        return await dbContext.Enrolments.AsNoTracking()
            .Where(x => x.StudentId == studentKey.Value && x.CourseId == courseKey.Value)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Course>> GetCoursesByStudentIdAsync(long studentId, CancellationToken cancellationToken = default)
    {
        var key = studentId.ToEntityId();
        if (key is null)
        {
            return [];
        }

        var courseIds = dbContext.Enrolments
            .Where(x => x.StudentId == key.Value && x.CourseId != null)
            .Select(x => x.CourseId!.Value);

        return await dbContext.Courses.AsNoTracking()
            .Where(c => courseIds.Contains(c.Id))
            .OrderBy(c => c.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Enrolment enrolment, CancellationToken cancellationToken = default)
    {
        await dbContext.Enrolments.AddAsync(enrolment, cancellationToken);
    }

    public void Remove(Enrolment enrolment)
    {
        dbContext.Enrolments.Remove(enrolment);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
