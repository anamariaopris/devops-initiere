using System.ComponentModel.DataAnnotations;

namespace OnlineShopApi.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field)]
public class NotBlankAttribute : ValidationAttribute
{
    public NotBlankAttribute() : base("must not be blank")
    {
    }

    public override bool IsValid(object? value) => value is string text && !string.IsNullOrWhiteSpace(text);
}
