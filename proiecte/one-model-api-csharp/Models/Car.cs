using System.ComponentModel.DataAnnotations;

namespace OneModelApi.Models;

public class Car
{
    public long Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Brand { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Model { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Color { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Size { get; set; }

    [Range(1886, int.MaxValue)]
    public int Year { get; set; }

    [Range(0, double.MaxValue)]
    public double Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Mileage { get; set; }

    [Required]
    [MaxLength(255)]
    public string FuelType { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Transmission { get; set; } = string.Empty;

    public bool Available { get; set; }
}
