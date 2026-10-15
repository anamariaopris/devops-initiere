using System.Text.RegularExpressions;

namespace OnlineSchoolApi.Security;

public class PathPattern
{
    private readonly Regex _regex;

    public PathPattern(string pattern)
    {
        var escaped = Regex.Escape(pattern.Trim())
            .Replace("/\\*\\*", "(/.*)?")
            .Replace("\\*\\*", ".*")
            .Replace("\\*", "[^/]*");
        _regex = new Regex($"^{escaped}/?$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    public bool Matches(string path) => _regex.IsMatch(path);
}
