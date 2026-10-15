using OnlineSchoolApi.Common.Exceptions;
using OnlineSchoolApi.Courses.Dtos;
using OnlineSchoolApi.Courses.Mappers;
using OnlineSchoolApi.Enrolments.Repositories;
using OnlineSchoolApi.Users.Students.Dtos;
using OnlineSchoolApi.Users.Students.Exceptions;
using OnlineSchoolApi.Users.Students.Mappers;
using OnlineSchoolApi.Users.Students.Repositories;

namespace OnlineSchoolApi.Users.Students.Services;

public class StudentQueryService(IStudentRepository studentRepository, IEnrolmentRepository enrolmentRepository) : IStudentQueryService
{
    public async Task<List<StudentResponse>> GetAllStudentsAsync(CancellationToken cancellationToken = default)
        => (await studentRepository.GetAllAsync(cancellationToken)).Select(StudentMapper.ToResponse).ToList();

    public async Task<StudentResponse> GetStudentByIdAsync(long studentId, CancellationToken cancellationToken = default)
    {
        var student = await studentRepository.GetByIdAsync(studentId, cancellationToken)
            ?? throw new StudentNotFoundException(studentId);

        return StudentMapper.ToResponse(student);
    }

    public async Task<StudentResponse> FindByEmailAsync(string? email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidRequestException("email must not be blank");
        }

        var student = await studentRepository.GetByEmailAsync(email, cancellationToken)
            ?? throw new StudentNotFoundException(email);

        return StudentMapper.ToResponse(student);
    }

    public async Task<List<StudentResponse>> SearchStudentsAsync(string? query, CancellationToken cancellationToken = default)
    {
        var normalized = query?.Trim() ?? string.Empty;
        if (normalized.Length == 0)
        {
            return [];
        }

        return (await studentRepository.SearchAsync(normalized, cancellationToken)).Select(StudentMapper.ToResponse).ToList();
    }

    public async Task<List<CourseResponse>> GetCoursesForStudentAsync(long studentId, CancellationToken cancellationToken = default)
    {
        if (!await studentRepository.ExistsByIdAsync(studentId, cancellationToken))
        {
            throw new StudentNotFoundException(studentId);
        }

        return (await enrolmentRepository.GetCoursesByStudentIdAsync(studentId, cancellationToken))
            .Select(CourseMapper.ToResponse)
            .ToList();
    }
}
