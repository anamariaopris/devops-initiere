using OnlineSchoolApi.Common.Exceptions;

namespace OnlineSchoolApi.Users.Exceptions;

public class UserNotFoundException(long userId) : NotFoundException($"user with id {userId} not found");
