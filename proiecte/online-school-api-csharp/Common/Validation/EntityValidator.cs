using System.ComponentModel.DataAnnotations;
using OnlineSchoolApi.Common.Exceptions;

namespace OnlineSchoolApi.Common.Validation;

public static class EntityValidator
{
    public static void Validate(object entity)
    {
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(entity, new ValidationContext(entity), results, validateAllProperties: true))
        {
            throw new ConstraintViolationException();
        }
    }
}
