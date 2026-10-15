using OnlineSchoolApi.Common.Exceptions;

namespace OnlineSchoolApi.Users.Administrators.Exceptions;

public class AdministratorNotFoundException : NotFoundException
{
    public AdministratorNotFoundException(long administratorId) : base($"administrator with id {administratorId} not found")
    {
    }

    public AdministratorNotFoundException(string email) : base($"administrator with email {email} not found")
    {
    }
}
