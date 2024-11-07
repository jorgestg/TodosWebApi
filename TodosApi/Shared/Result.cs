using System.Diagnostics;

namespace TodosApi.Shared;

public readonly struct Result<T>
{
    private readonly int _status;
    private readonly T? _value;
    private readonly Error? _error;

    private Result(int status, T? value, Error? error)
    {
        _status = status;
        _value = value;
        _error = error;
    }

    public bool IsSuccess =>
        _status switch
        {
            -1 => false,
            0 => throw new InvalidOperationException("Result is not initialized"),
            1 => true,
            _ => throw new UnreachableException(),
        };

    public T OkValue => IsSuccess ? _value! : throw new InvalidOperationException("Result.IsSuccess is false");

    public Error Error => IsSuccess ? throw new InvalidOperationException("Result.IsSuccess is true") : _error!;

    public static Result<T> FromValue(T value) => new(1, value, null);

    public static Result<T> FromError(Error error) => new(-1, default, error);

    public static implicit operator Result<T>(T value) => FromValue(value);

    public static implicit operator Result<T>(Error error) => FromError(error);

    public override string ToString()
    {
        return IsSuccess ? $"Success: {OkValue}" : $"Failure: {Error}";
    }
}
