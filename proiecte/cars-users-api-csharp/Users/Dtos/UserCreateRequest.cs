using System.ComponentModel.DataAnnotations;

namespace OneToManyApi.Dtos;

public record UserCreateRequest(
    [Required, MinLength(1)] string Name,
    [Required, EmailAddress] string Email
);
