using OnlineSchoolApi.Common.Exceptions;

namespace OnlineSchoolApi.Users.Students.Exceptions;

public class StudentAlreadyExistsException(string email) : ConflictException($"Student already exists with email={email}");
