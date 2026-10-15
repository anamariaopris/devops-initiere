using System.ComponentModel.DataAnnotations;

namespace OneToManyApi.Dtos;

public record CarUpdateRequest(
    [Required, MinLength(1)] string Brand,
    [Required, MinLength(1)] string Model,
    [Range(1886, int.MaxValue)] int Year,
    [Range(1, long.MaxValue)] long UserId
);
