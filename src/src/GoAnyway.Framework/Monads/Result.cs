using System.Diagnostics.CodeAnalysis;

namespace GoAnyway.Framework.Monads;

public readonly record struct Result<T>
{
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess { get; }

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => !IsSuccess;

    public T? Value { get; }

    public Error? Error { get; }

    private Result(
        bool isSuccess, 
        T? value,
        Error? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new(
            isSuccess: true, 
            value, 
            error: null
        );
    }

    public static Result<T> Failure(Error error)
    {
        return new(
            isSuccess: false,
            value: default, 
            error
        );
    }

    public override string ToString()
    {
        return IsSuccess ? $"Success({Value})" : $"Failure({Error})";
    }

    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }

    public static implicit operator Result<T>(Error error)
    {
        return Failure(error);
    }
}

public static class Result
{
    public static Result<T> Failure<T>(Error error)
    {
        return Result<T>.Failure(error);
    }
}