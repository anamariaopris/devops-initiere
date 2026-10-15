namespace OnlineShopApi.Security;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Secret { get; set; } = string.Empty;
    public long AccessTokenExpirationMs { get; set; } = 900000;
    public long RefreshTokenExpirationMs { get; set; } = 604800000;
}
