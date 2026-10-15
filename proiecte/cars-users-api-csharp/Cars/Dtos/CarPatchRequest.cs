using System.ComponentModel.DataAnnotations;

namespace OneToManyApi.Dtos;

public class CarPatchRequest : IValidatableObject
{
    [MinLength(1)]
    public string? Brand { get; init; }

    [MinLength(1)]
    public string? Model { get; init; }

    [Range(1886, int.MaxValue)]
    public int? Year { get; init; }

    [Range(1, long.MaxValue)]
    public long? UserId { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Brand) && string.IsNullOrWhiteSpace(Model) && Year is null && UserId is null)
        {
            yield return new ValidationResult(
                "At least one field must be provided for patch.",
                [nameof(Brand), nameof(Model), nameof(Year), nameof(UserId)]
            );
        }
    }
}
