namespace OnlineSchoolApi.Users.Administrators;

public record AdministratorResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? EmployeeCode,
    string? Department,
    string? JobTitle);

public static class AdministratorMappings
{
    public static AdministratorResponse ToResponse(this Administrator administrator) => new(
        administrator.Id, administrator.FirstName, administrator.LastName, administrator.Email,
        administrator.EmployeeCode, administrator.Department, administrator.JobTitle);
}
