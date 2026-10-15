using System.ComponentModel.DataAnnotations;

namespace OneToManyApi.Models;

public class Car
{
    public long Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Brand { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Model { get; set; } = string.Empty;

    [Range(1886, int.MaxValue)]
    public int Year { get; set; }

    public long UserId { get; set; }
    public User? User { get; set; }
}
