using System.ComponentModel.DataAnnotations;

namespace OnlineSchoolApi.Common.Validation;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public class PositiveAttribute : ValidationAttribute
{
    public PositiveAttribute() : base(ValidationMessages.ConstraintFailed)
    {
    }

    public override bool IsValid(object? value) => value switch
    {
        null => true,
        long number => number > 0,
        int number => number > 0,
        _ => false
    };
}
