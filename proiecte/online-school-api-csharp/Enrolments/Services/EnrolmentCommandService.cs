using OnlineSchoolApi.Common.Exceptions;
using OnlineSchoolApi.Courses.Exceptions;
using OnlineSchoolApi.Courses.Repositories;
using OnlineSchoolApi.Enrolments.Dtos;
using OnlineSchoolApi.Enrolments.Exceptions;
using OnlineSchoolApi.Enrolments.Mappers;
using OnlineSchoolApi.Enrolments.Models;
using OnlineSchoolApi.Enrolments.Repositories;
using OnlineSchoolApi.Users.Students.Exceptions;
using OnlineSchoolApi.Users.Students.Repositories;

namespace OnlineSchoolApi.Enrolments.Services;

public class EnrolmentCommandService(
    IEnrolmentRepository enrolmentRepository,
    IStudentRepository studentRepository,
    ICourseRepository courseRepository) : IEnrolmentCommandService
{
    public async Task<EnrolmentResponse> EnrollStudentAsync(EnrolmentCreateRequest? request, CancellationToken cancellationToken = default)
    {
        if (request?.StudentId is null || request.CourseId is null)
        {
            throw new RequestBodyMissingException();
        }

        var studentId = request.StudentId.Value;
        var courseId = request.CourseId.Value;

        var course = await courseRepository.GetByIdAsync(courseId, cancellationToken)
            ?? throw new CourseNotFoundException(courseId);
        var student = await studentRepository.GetByIdAsync(studentId, cancellationToken)
            ?? throw new StudentNotFoundException(studentId);

        var existing = await enrolmentRepository.GetByStudentIdAndCourseIdAsync(studentId, courseId, cancellationToken);
        if (existing.Count > 0)
        {
            throw new EnrolmentAlreadyExistsException(studentId, courseId);
        }

        var enrolment = new Enrolment { StudentId = student.Id, CourseId = course.Id };
        await enrolmentRepository.AddAsync(enrolment, cancellationToken);
        await enrolmentRepository.SaveChangesAsync(cancellationToken);
        return EnrolmentMapper.ToResponse(enrolment);
    }

    public async Task<EnrolmentResponse> UpdateEnrolmentAsync(long enrolmentId, EnrolmentUpdateRequest? request, CancellationToken cancellationToken = default)
    {
        var enrolment = await enrolmentRepository.GetTrackedByIdAsync(enrolmentId, cancellationToken)
            ?? throw new EnrolmentNotFoundException(enrolmentId);

        if (request is null)
        {
            throw new RequestBodyMissingException();
        }

        if (request.StudentId is null && request.CourseId is null)
        {
            throw new EmptyUpdateRequestException();
        }

        if (request.StudentId is not null)
        {
            var student = await studentRepository.GetByIdAsync(request.StudentId.Value, cancellationToken)
                ?? throw new StudentNotFoundException(request.StudentId.Value);
            enrolment.StudentId = student.Id;
        }

        if (request.CourseId is not null)
        {
            var course = await courseRepository.GetByIdAsync(request.CourseId.Value, cancellationToken)
                ?? throw new CourseNotFoundException(request.CourseId.Value);
            enrolment.CourseId = course.Id;
        }

        await enrolmentRepository.SaveChangesAsync(cancellationToken);
        return EnrolmentMapper.ToResponse(enrolment);
    }

    public async Task<EnrolmentResponse> DeleteEnrolmentAsync(long enrolmentId, CancellationToken cancellationToken = default)
    {
        var enrolment = await enrolmentRepository.GetTrackedByIdAsync(enrolmentId, cancellationToken)
            ?? throw new EnrolmentNotFoundException(enrolmentId);

        var response = EnrolmentMapper.ToResponse(enrolment);
        enrolmentRepository.Remove(enrolment);
        await enrolmentRepository.SaveChangesAsync(cancellationToken);
        return response;
    }
}
