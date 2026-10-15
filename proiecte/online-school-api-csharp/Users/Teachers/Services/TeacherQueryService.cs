using OnlineSchoolApi.Common.Exceptions;
using OnlineSchoolApi.Users.Teachers.Dtos;
using OnlineSchoolApi.Users.Teachers.Exceptions;
using OnlineSchoolApi.Users.Teachers.Mappers;
using OnlineSchoolApi.Users.Teachers.Repositories;

namespace OnlineSchoolApi.Users.Teachers.Services;

public class TeacherQueryService(ITeacherRepository teacherRepository) : ITeacherQueryService
{
    public async Task<List<TeacherResponse>> GetAllTeachersAsync(CancellationToken cancellationToken = default)
        => (await teacherRepository.GetAllAsync(cancellationToken)).Select(TeacherMapper.ToResponse).ToList();

    public async Task<TeacherResponse> GetTeacherByIdAsync(long teacherId, CancellationToken cancellationToken = default)
    {
        var teacher = await teacherRepository.GetByIdAsync(teacherId, cancellationToken)
            ?? throw new TeacherNotFoundException(teacherId);

        return TeacherMapper.ToResponse(teacher);
    }

    public async Task<TeacherResponse> FindByEmailAsync(string? email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidRequestException("email must not be blank");
        }

        var teacher = await teacherRepository.GetByEmailAsync(email, cancellationToken)
            ?? throw new TeacherNotFoundException(email);

        return TeacherMapper.ToResponse(teacher);
    }

    public async Task<List<TeacherResponse>> SearchTeachersAsync(string? query, CancellationToken cancellationToken = default)
    {
        var normalized = query?.Trim() ?? string.Empty;
        if (normalized.Length == 0)
        {
            return [];
        }

        return (await teacherRepository.SearchAsync(normalized, cancellationToken)).Select(TeacherMapper.ToResponse).ToList();
    }
}
