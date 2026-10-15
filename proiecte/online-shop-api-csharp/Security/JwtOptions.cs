using System.ComponentModel.DataAnnotations;

namespace OnlineShopApi.Security;

public class JwtOptions
{
    [Required(ErrorMessage = "Jwt:Secret is missing. Set it with the Jwt__Secret environment variable.")]
    public string Secret { get; set; } = string.Empty;

    [Range(1, long.MaxValue)]
    public long AccessTokenExpirationMs { get; set; } = 900000;

    [Range(1, long.MaxValue)]
    public long RefreshTokenExpirationMs { get; set; } = 604800000;
}
