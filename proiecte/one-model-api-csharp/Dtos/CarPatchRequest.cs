namespace OneModelApi.Dtos;

public record CarPatchRequest(
    string? Brand,
    string? Model,
    string? Color,
    int? Size,
    int? Year,
    double? Price,
    int? Mileage,
    string? FuelType,
    string? Transmission,
    bool? Available
);
