using System.Runtime.CompilerServices;

namespace GoAnyway.Framework.Assertion;

public static class GuidAssertion
{
    public static Guid ThrowIfEmpty(
        this Guid value,
        [CallerArgumentExpression(nameof(value))]
        string? paramName = default)
    {
        return value != Guid.Empty
            ? value
            : throw new ArgumentException(null, paramName);
    }
}