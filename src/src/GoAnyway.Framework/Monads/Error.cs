namespace GoAnyway.Framework.Monads;

public readonly record struct Error(string Message)
{
    public static Error Create(string message)
    {
        return new(message);
    }

    public static implicit operator Error(string message)
    {
        return new(message);
    }
}