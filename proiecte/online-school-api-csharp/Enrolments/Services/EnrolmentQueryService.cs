using OnlineSchoolApi.Enrolments.Dtos;
using OnlineSchoolApi.Enrolments.Exceptions;
using OnlineSchoolApi.Enrolments.Mappers;
using OnlineSchoolApi.Enrolments.Models;
using OnlineSchoolApi.Enrolments.Repositories;

namespace OnlineSchoolApi.Enrolments.Services;

public class EnrolmentQueryService(IEnrolmentRepository enrolmentRepository) : IEnrolmentQueryService
{
    public async Task<List<EnrolmentResponse>> GetAllEnrolmentsAsync(CancellationToken cancellationToken = default)
        => ToResponses(await enrolmentRepository.GetAllAsync(cancellationToken));

    public async Task<EnrolmentResponse> GetEnrolmentByIdAsync(long enrolmentId, CancellationToken cancellationToken = default)
    {
        var enrolment = await enrolmentRepository.GetByIdAsync(enrolmentId, cancellationToken)
            ?? throw new EnrolmentNotFoundException(enrolmentId);

        return EnrolmentMapper.ToResponse(enrolment);
    }

    public async Task<List<EnrolmentResponse>> FindByStudentIdAsync(long studentId, CancellationToken cancellationToken = default)
        => ToResponses(await enrolmentRepository.GetByStudentIdAsync(studentId, cancellationToken));

    public async Task<List<EnrolmentResponse>> FindByCourseIdAsync(long courseId, CancellationToken cancellationToken = default)
        => ToResponses(await enrolmentRepository.GetByCourseIdAsync(courseId, cancellationToken));

    public async Task<List<EnrolmentResponse>> SearchEnrolmentsAsync(long? studentId, long? courseId, CancellationToken cancellationToken = default)
    {
        var enrolments = (studentId, courseId) switch
        {
            ({ } s, { } c) => await enrolmentRepository.GetByStudentIdAndCourseIdAsync(s, c, cancellationToken),
            ({ } s, null) => await enrolmentRepository.GetByStudentIdAsync(s, cancellationToken),
            (null, { } c) => await enrolmentRepository.GetByCourseIdAsync(c, cancellationToken),
            _ => await enrolmentRepository.GetAllAsync(cancellationToken)
        };

        return ToResponses(enrolments);
    }

    private static List<EnrolmentResponse> ToResponses(IEnumerable<Enrolment> enrolments)
        => enrolments.Select(EnrolmentMapper.ToResponse).ToList();
}
