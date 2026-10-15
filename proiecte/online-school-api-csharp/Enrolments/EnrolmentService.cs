using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Courses;
using OnlineSchoolApi.Data;
using OnlineSchoolApi.Errors;
using OnlineSchoolApi.Users.Students;

namespace OnlineSchoolApi.Enrolments;

public class EnrolmentService(AppDbContext db)
{
    public Task<List<EnrolmentResponse>> GetAllAsync(CancellationToken cancellationToken) =>
        ListAsync(db.Enrolments, cancellationToken);

    public async Task<EnrolmentResponse> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var enrolment = await db.Enrolments.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
            ?? throw NotFound(id);
        return enrolment.ToResponse();
    }

    public Task<List<EnrolmentResponse>> SearchAsync(long? studentId, long? courseId, CancellationToken cancellationToken)
    {
        var query = db.Enrolments.AsQueryable();
        if (studentId is not null) query = query.Where(e => e.StudentId == studentId);
        if (courseId is not null) query = query.Where(e => e.CourseId == courseId);
        return ListAsync(query, cancellationToken);
    }

    public async Task<EnrolmentResponse> CreateAsync(EnrolmentCreateRequest request, CancellationToken cancellationToken)
    {
        var course = await RequireCourseAsync(request.CourseId, cancellationToken);
        var student = await RequireStudentAsync(request.StudentId, cancellationToken);
        if (await db.Enrolments.AnyAsync(e => e.StudentId == student.Id && e.CourseId == course.Id, cancellationToken))
        {
            throw new ConflictException($"Enrolment already exists for studentId={request.StudentId} and courseId={request.CourseId}");
        }

        var enrolment = new Enrolment { StudentId = student.Id, CourseId = course.Id };
        db.Enrolments.Add(enrolment);
        await db.SaveChangesAsync(cancellationToken);
        return enrolment.ToResponse();
    }

    public async Task<EnrolmentResponse> UpdateAsync(long id, EnrolmentUpdateRequest request, CancellationToken cancellationToken)
    {
        var enrolment = await FindAsync(id, cancellationToken);
        if (request is { StudentId: null, CourseId: null })
        {
            throw BadRequestException.EmptyUpdate();
        }

        if (request.StudentId is not null)
        {
            enrolment.StudentId = (await RequireStudentAsync(request.StudentId.Value, cancellationToken)).Id;
        }
        if (request.CourseId is not null)
        {
            enrolment.CourseId = (await RequireCourseAsync(request.CourseId.Value, cancellationToken)).Id;
        }

        await db.SaveChangesAsync(cancellationToken);
        return enrolment.ToResponse();
    }

    public async Task<EnrolmentResponse> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var enrolment = await FindAsync(id, cancellationToken);
        db.Enrolments.Remove(enrolment);
        await db.SaveChangesAsync(cancellationToken);
        return enrolment.ToResponse();
    }

    private static async Task<List<EnrolmentResponse>> ListAsync(IQueryable<Enrolment> query, CancellationToken cancellationToken)
    {
        var enrolments = await query.AsNoTracking().OrderBy(e => e.Id).ToListAsync(cancellationToken);
        return enrolments.Select(e => e.ToResponse()).ToList();
    }

    private async Task<Enrolment> FindAsync(long id, CancellationToken cancellationToken) =>
        await db.Enrolments.FirstOrDefaultAsync(e => e.Id == id, cancellationToken) ?? throw NotFound(id);

    private async Task<Student> RequireStudentAsync(long studentId, CancellationToken cancellationToken) =>
        await db.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken)
            ?? throw new NotFoundException($"Student not found with id={studentId}");

    private async Task<Course> RequireCourseAsync(long courseId, CancellationToken cancellationToken) =>
        await db.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.Id == courseId, cancellationToken)
            ?? throw new NotFoundException($"Course not found with id={courseId}");

    private static NotFoundException NotFound(long id) => new($"Enrolment not found with id={id}");
}
