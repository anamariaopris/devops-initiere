using OnlineSchoolApi.Common.Exceptions;

namespace OnlineSchoolApi.Users.Students.Exceptions;

public class StudentNotFoundException : NotFoundException
{
    public StudentNotFoundException(long studentId) : base($"Student not found with id={studentId}")
    {
    }

    public StudentNotFoundException(string email) : base($"Student not found with email={email}")
    {
    }
}
