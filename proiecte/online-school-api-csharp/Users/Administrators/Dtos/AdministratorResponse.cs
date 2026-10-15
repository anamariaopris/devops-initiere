namespace OnlineSchoolApi.Users.Administrators.Dtos;

public record AdministratorResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? EmployeeCode,
    string? Department,
    string? JobTitle
);
