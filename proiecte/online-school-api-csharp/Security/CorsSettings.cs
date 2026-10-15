namespace OnlineSchoolApi.Security;

public class CorsSettings
{
    public const string SectionName = "Cors";

    public List<string> AllowedOriginPatterns { get; set; } = [];
    public string AdditionalOrigins { get; set; } = string.Empty;
    public List<string> AllowedMethods { get; set; } = [];
    public List<string> AllowedHeaders { get; set; } = [];
    public List<string> ExposedHeaders { get; set; } = [];
    public bool AllowCredentials { get; set; } = true;
    public int MaxAgeSeconds { get; set; } = 3600;

    public IEnumerable<string> AllOriginPatterns() => AllowedOriginPatterns
        .Concat(AdditionalOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        .Where(p => !string.IsNullOrWhiteSpace(p))
        .Distinct(StringComparer.OrdinalIgnoreCase);
}
