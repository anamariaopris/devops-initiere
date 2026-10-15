using System.ComponentModel.DataAnnotations;

namespace OneToManyApi.Dtos;

public class UserPatchRequest : IValidatableObject
{
    [MinLength(1)]
    public string? Name { get; init; }

    [EmailAddress]
    public string? Email { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(Email))
        {
            yield return new ValidationResult(
                "At least one field must be provided for patch.",
                [nameof(Name), nameof(Email)]
            );
        }
    }
}
