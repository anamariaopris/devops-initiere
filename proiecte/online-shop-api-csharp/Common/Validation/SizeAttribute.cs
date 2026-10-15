using System.ComponentModel.DataAnnotations;

namespace OnlineShopApi.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field)]
public class SizeAttribute : ValidationAttribute
{
    private readonly int _min;
    private readonly int _max;

    public SizeAttribute(int min, int max) : base($"size must be between {min} and {max}")
    {
        _min = min;
        _max = max;
    }

    public override bool IsValid(object? value) => value is not string text || (text.Length >= _min && text.Length <= _max);
}
