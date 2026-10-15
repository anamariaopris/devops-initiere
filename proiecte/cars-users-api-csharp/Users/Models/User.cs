using System.ComponentModel.DataAnnotations;

namespace OneToManyApi.Models;

public class User
{
    public long Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    public ICollection<Car> Cars { get; set; } = new List<Car>();
}
