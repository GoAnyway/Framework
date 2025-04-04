using System.Runtime.CompilerServices;

namespace GoAnyway.Framework.Assertion;

public static class StringAssertion
{
    public static string ThrowIfNullOrEmpty(
        this string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(value, paramName);
        return value;
    }
}