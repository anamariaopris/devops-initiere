using OnlineSchoolApi.Users.Administrators.Dtos;
using OnlineSchoolApi.Users.Administrators.Models;

namespace OnlineSchoolApi.Users.Administrators.Mappers;

public static class AdministratorMapper
{
    public static AdministratorResponse ToResponse(Administrator administrator) => new(
        administrator.Id,
        administrator.FirstName,
        administrator.LastName,
        administrator.Email,
        administrator.EmployeeCode,
        administrator.Department,
        administrator.JobTitle
    );
}
