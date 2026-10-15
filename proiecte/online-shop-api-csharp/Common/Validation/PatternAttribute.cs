using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OnlineShopApi.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field)]
public class PatternAttribute : ValidationAttribute
{
    private readonly Regex _regex;

    public PatternAttribute(string pattern, string message) : base(message)
    {
        _regex = new Regex(pattern, RegexOptions.CultureInvariant);
    }

    public override bool IsValid(object? value) => value is not string text || _regex.IsMatch(text);
}
