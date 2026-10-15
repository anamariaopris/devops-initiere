namespace mock_car_source_api.Models;

public class ExternalCarDto
{
    public string ExternalId { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal PriceEur { get; set; }
    public int MileageKm { get; set; }
    public string FuelType { get; set; } = string.Empty;
    public string Transmission { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public DateTime PublishedAtUtc { get; set; }
}