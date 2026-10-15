namespace OneModelApi.Dtos;

public record CarResponse(
    long Id,
    string Brand,
    string Model,
    string Color,
    int Size,
    int Year,
    double Price,
    int Mileage,
    string FuelType,
    string Transmission,
    bool Available
);
