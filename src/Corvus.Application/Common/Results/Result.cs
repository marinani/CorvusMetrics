namespace Corvus.Application.Common.Results;

public sealed class Result
{
    private Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }

    public static Result Success() => new(isSuccess: true, error: null);

    public static Result Failure(Error error) => new(isSuccess: false, error: error);
}