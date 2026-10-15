using System.ComponentModel.DataAnnotations;

namespace OnlineSchoolApi.Users.Models;

public abstract class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(50, MinimumLength = 3)]
    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public string? PasswordHash { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? LastLoginAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<UserPermission> Permissions { get; set; } = new List<UserPermission>();

    public List<string> GetAuthorities() => Permissions
        .Select(p => p.Permission.ToAuthority())
        .OrderBy(a => a, StringComparer.Ordinal)
        .ToList();
}
