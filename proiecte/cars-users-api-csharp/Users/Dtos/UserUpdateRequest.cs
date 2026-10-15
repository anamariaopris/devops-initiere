using System.ComponentModel.DataAnnotations;

namespace OneToManyApi.Dtos;

public record UserUpdateRequest(
    [Required, MinLength(1)] string Name,
    [Required, EmailAddress] string Email
);
