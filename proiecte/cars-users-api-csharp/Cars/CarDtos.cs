using System.ComponentModel.DataAnnotations;

namespace OneToManyApi.Cars;

public record CarRequest(
    [Required, MinLength(1), MaxLength(255)] string Brand,
    [Required, MinLength(1), MaxLength(255)] string Model,
    [Range(1886, int.MaxValue)] int Year,
    [Range(1, long.MaxValue)] long UserId);

public record CarPatchRequest(
    [MinLength(1), MaxLength(255)] string? Brand,
    [MinLength(1), MaxLength(255)] string? Model,
    [Range(1886, int.MaxValue)] int? Year,
    [Range(1, long.MaxValue)] long? UserId);

public record CarSearchRequest(string? Brand, string? Model, long? UserId, int? MinYear, int? MaxYear);

public record CarResponse(long Id, string Brand, string Model, int Year, long UserId);

public record CarListResponse(IEnumerable<CarResponse> Cars);

public record UserCarsResponse(long UserId, IEnumerable<CarResponse> Cars);

public static class CarMappings
{
    public static CarResponse ToResponse(this Car car) => new(car.Id, car.Brand, car.Model, car.Year, car.UserId);
}
