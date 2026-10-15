using System.ComponentModel.DataAnnotations;
using OneToManyApi.Cars;

namespace OneToManyApi.Users;

public record UserRequest(
    [Required, MinLength(1), MaxLength(255)] string Name,
    [Required, EmailAddress, MaxLength(255)] string Email);

public record UserPatchRequest(
    [MinLength(1), MaxLength(255)] string? Name,
    [EmailAddress, MaxLength(255)] string? Email);

public record UserSearchRequest(string? Name, string? Email);

public record UserResponse(long Id, string Name, string Email);

public record UserWithCarsResponse(long Id, string Name, string Email, IEnumerable<CarResponse> Cars);

public record UserWithCarsListResponse(IEnumerable<UserWithCarsResponse> Users);

public static class UserMappings
{
    public static UserResponse ToResponse(this User user) => new(user.Id, user.Name, user.Email);

    public static UserWithCarsResponse ToWithCarsResponse(this User user) =>
        new(user.Id, user.Name, user.Email, user.Cars.Select(c => c.ToResponse()));
}
