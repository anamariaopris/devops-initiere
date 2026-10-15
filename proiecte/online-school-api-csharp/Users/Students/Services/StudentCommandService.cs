using OnlineSchoolApi.Common.Exceptions;
using OnlineSchoolApi.Users.Students.Dtos;
using OnlineSchoolApi.Users.Students.Exceptions;
using OnlineSchoolApi.Users.Students.Mappers;
using OnlineSchoolApi.Users.Students.Repositories;

namespace OnlineSchoolApi.Users.Students.Services;

public class StudentCommandService(IStudentRepository studentRepository) : IStudentCommandService
{
    public async Task<StudentResponse> CreateStudentAsync(StudentCreateRequest? request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new RequestBodyMissingException();
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new InvalidRequestException("email must not be blank");
        }

        if (await studentRepository.ExistsByEmailAsync(request.Email.ToLower(), cancellationToken))
        {
            throw new StudentAlreadyExistsException(request.Email);
        }

        var student = StudentMapper.ToEntity(request);
        await studentRepository.AddAsync(student, cancellationToken);
        await studentRepository.SaveChangesAsync(cancellationToken);

        return StudentMapper.ToResponse(student);
    }

    public async Task<StudentResponse> UpdateStudentAsync(long studentId, StudentUpdateRequest? request, CancellationToken cancellationToken = default)
    {
        var student = await studentRepository.GetTrackedByIdAsync(studentId, cancellationToken)
            ?? throw new StudentNotFoundException(studentId);

        if (request is null)
        {
            throw new RequestBodyMissingException();
        }

        if (request.FirstName is null && request.LastName is null && request.Email is null)
        {
            throw new EmptyUpdateRequestException();
        }

        if (request.FirstName is not null)
        {
            student.FirstName = request.FirstName;
        }

        if (request.LastName is not null)
        {
            student.LastName = request.LastName;
        }

        if (request.Email is not null)
        {
            student.Email = request.Email;
        }

        await studentRepository.SaveChangesAsync(cancellationToken);
        return StudentMapper.ToResponse(student);
    }

    public async Task<StudentResponse> DeleteStudentAsync(long studentId, CancellationToken cancellationToken = default)
    {
        var student = await studentRepository.GetTrackedByIdAsync(studentId, cancellationToken)
            ?? throw new StudentNotFoundException(studentId);

        var response = StudentMapper.ToResponse(student);
        studentRepository.Remove(student);
        await studentRepository.SaveChangesAsync(cancellationToken);
        return response;
    }
}
