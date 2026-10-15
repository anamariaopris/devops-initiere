using System.ComponentModel.DataAnnotations;

namespace OnlineSchoolApi.Auth;

public class JwtOptions : IValidatableObject
{
    [Required]
    public string Secret { get; set; } = string.Empty;

    [Range(60, long.MaxValue)]
    public long ExpirationSeconds { get; set; } = 86400;

    public byte[] KeyBytes()
    {
        var buffer = new byte[Secret.Length];
        return Convert.TryFromBase64String(Secret, buffer, out var length) ? buffer[..length] : [];
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (KeyBytes().Length < 32)
        {
            yield return new ValidationResult("Jwt:Secret must be Base64 and decode to at least 32 bytes.", [nameof(Secret)]);
        }
    }
}
