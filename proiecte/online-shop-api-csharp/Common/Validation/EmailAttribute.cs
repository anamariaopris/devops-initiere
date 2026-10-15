using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OnlineShopApi.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field)]
public partial class EmailAttribute : ValidationAttribute
{
    public EmailAttribute() : base("must be a well-formed email address")
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is not string text || text.Length == 0)
        {
            return true;
        }

        var at = text.LastIndexOf('@');
        if (at <= 0 || at == text.Length - 1)
        {
            return false;
        }

        return LocalPart().IsMatch(text[..at]) && DomainPart().IsMatch(text[(at + 1)..]);
    }

    [GeneratedRegex(@"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~\-\u0080-￿]+(\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~\-\u0080-￿]+)*$")]
    private static partial Regex LocalPart();

    [GeneratedRegex(@"^([a-zA-Z0-9\u0080-￿]([a-zA-Z0-9\-\u0080-￿]{0,61}[a-zA-Z0-9\u0080-￿])?)(\.[a-zA-Z0-9\u0080-￿]([a-zA-Z0-9\-\u0080-￿]{0,61}[a-zA-Z0-9\u0080-￿])?)*$|^\[[0-9a-fA-F:.]+\]$")]
    private static partial Regex DomainPart();
}
