using System.ComponentModel.DataAnnotations;

namespace OneModelApi.Dtos;

public record CarPutRequest(
    [Required, MinLength(1)] string Brand,
    [Required, MinLength(1)] string Model,
    [Required, MinLength(1)] string Color,
    [Range(1, int.MaxValue)] int Size,
    [Range(1886, int.MaxValue)] int Year,
    [Range(0, double.MaxValue)] double Price,
    [Range(0, int.MaxValue)] int Mileage,
    [Required, MinLength(1)] string FuelType,
    [Required, MinLength(1)] string Transmission,
    bool Available
);
