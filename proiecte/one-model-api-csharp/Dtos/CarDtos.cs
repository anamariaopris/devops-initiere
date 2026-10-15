using System.ComponentModel.DataAnnotations;
using OneModelApi.Models;

namespace OneModelApi.Dtos;

public record CarRequest(
    [Required, MaxLength(255)] string Brand,
    [Required, MaxLength(255)] string Model,
    [Required, MaxLength(255)] string Color,
    [Range(1, int.MaxValue)] int Size,
    [Range(1886, int.MaxValue)] int Year,
    [Range(0, 99999999.99)] decimal Price,
    [Range(0, int.MaxValue)] int Mileage,
    [Required, MaxLength(255)] string FuelType,
    [Required, MaxLength(255)] string Transmission,
    bool Available);

public record CarPatchRequest(
    [MinLength(1), MaxLength(255)] string? Brand,
    [MinLength(1), MaxLength(255)] string? Model,
    [MinLength(1), MaxLength(255)] string? Color,
    [Range(1, int.MaxValue)] int? Size,
    [Range(1886, int.MaxValue)] int? Year,
    [Range(0, 99999999.99)] decimal? Price,
    [Range(0, int.MaxValue)] int? Mileage,
    [MinLength(1), MaxLength(255)] string? FuelType,
    [MinLength(1), MaxLength(255)] string? Transmission,
    bool? Available);

public record CarSearchRequest(
    string? Brand,
    string? Model,
    string? Color,
    int? MinYear,
    int? MaxYear,
    decimal? MaxPrice,
    int? MaxMileage,
    string? FuelType,
    string? Transmission,
    bool? Available);

public record CarResponse(
    long Id,
    string Brand,
    string Model,
    string Color,
    int Size,
    int Year,
    decimal Price,
    int Mileage,
    string FuelType,
    string Transmission,
    bool Available);

public record CarListResponse(IEnumerable<CarResponse> Cars);

public static class CarMappings
{
    public static CarResponse ToResponse(this Car car) => new(
        car.Id, car.Brand, car.Model, car.Color, car.Size, car.Year,
        car.Price, car.Mileage, car.FuelType, car.Transmission, car.Available);
}
