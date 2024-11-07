using FluentValidation.Results;

namespace TodosApi.Shared;

public abstract class Error;

public sealed class NotFoundError : Error
{
    public static readonly NotFoundError Instance = new();
}

public sealed class ValidationError(IEnumerable<ValidationFailure> failures) : Error
{
    public IEnumerable<ValidationFailure> Failures { get; } = failures;
}
