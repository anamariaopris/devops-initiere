using OneModelApi.Dtos;
using OneModelApi.Models;

namespace OneModelApi.Services;

public static class CarMapper
{
    public static CarResponse ToResponse(Car car) => new(
        car.Id,
        car.Brand,
        car.Model,
        car.Color,
        car.Size,
        car.Year,
        car.Price,
        car.Mileage,
        car.FuelType,
        car.Transmission,
        car.Available
    );
}
