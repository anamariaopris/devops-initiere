namespace OnlineSchoolApi.Users.Permissions.Dtos;

public record UserPermissionsResponse(long UserId, List<string> Permissions);
