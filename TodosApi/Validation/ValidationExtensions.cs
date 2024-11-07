using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace TodosApi.Validation;

public static class ValidationExtensions
{
    public static bool IsValid<T>(
        this IValidator<T> validator,
        T value,
        [MaybeNullWhen(true)] out ValidationError validationError
    )
    {
        var validationResult = validator.Validate(value);
        if (validationResult.IsValid)
        {
            validationError = null;
            return true;
        }

        validationError = new ValidationError(validationResult.Errors);
        return false;
    }
}
