namespace OnlineSchoolApi.Users.Teachers.Dtos;

public record TeacherResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? EmployeeCode,
    string? Specialization,
    string? Title
);
