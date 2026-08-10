namespace Corvus.Application.Common.Results;

public sealed class Result<TValue>
{
    private Result(TValue value)
    {
        Value = value;
        Error = null;
    }

    private Result(Error error)
    {
        Error = error;
        Value = default!;
    }

    public TValue Value { get; }

    public Error? Error { get; }

    public bool IsSuccess => Error is null;

    public bool IsFailure => !IsSuccess;

    public static Result<TValue> Success(TValue value) => new(value);

    public static Result<TValue> Failure(Error error) => new(error);

    public static implicit operator Result<TValue>(TValue value) => new(value);
}