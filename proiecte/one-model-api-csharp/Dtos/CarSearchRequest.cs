namespace OneModelApi.Dtos;

public class CarSearchRequest
{
    public string? Brand { get; init; }
    public string? Model { get; init; }
    public string? Color { get; init; }
    public int? MinYear { get; init; }
    public int? MaxYear { get; init; }
    public double? MaxPrice { get; init; }
    public int? MaxMileage { get; init; }
    public string? FuelType { get; init; }
    public string? Transmission { get; init; }
    public bool? Available { get; init; }
}
