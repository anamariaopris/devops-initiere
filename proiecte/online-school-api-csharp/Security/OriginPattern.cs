using System.Text.RegularExpressions;

namespace OnlineSchoolApi.Security;

public class OriginPattern
{
    private const string AnyPort = "(:[0-9]{1,5})?";
    private const string AnyHostLabels = "[A-Za-z0-9-]+(\\.[A-Za-z0-9-]+)*";

    private readonly bool _matchesAny;
    private readonly Regex? _regex;

    public OriginPattern(string pattern)
    {
        var value = pattern.Trim().TrimEnd('/');
        if (value == "*")
        {
            _matchesAny = true;
            return;
        }

        var regex = Regex.Escape(value);
        if (regex.EndsWith(":\\*"))
        {
            regex = regex[..^3] + AnyPort;
        }

        regex = regex.Replace("\\*", AnyHostLabels);
        _regex = new Regex($"^{regex}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    public bool Matches(string origin) => _matchesAny || _regex!.IsMatch(origin.TrimEnd('/'));
}
