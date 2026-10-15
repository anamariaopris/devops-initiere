using System.Text.RegularExpressions;

namespace OnlineShopApi.Security;

public static class CorsOriginMatcher
{
    public static List<Regex> Parse(string? csv)
    {
        var patterns = string.IsNullOrWhiteSpace(csv)
            ? ["*"]
            : csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return patterns
            .Select(p => new Regex("^" + Regex.Escape(p).Replace("\\*", ".*") + "$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
            .ToList();
    }

    public static bool IsAllowed(List<Regex> patterns, string origin) => patterns.Any(p => p.IsMatch(origin));
}
