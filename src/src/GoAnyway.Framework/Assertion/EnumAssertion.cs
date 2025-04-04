using System.Runtime.CompilerServices;

namespace GoAnyway.Framework.Assertion;

public static class EnumAssertion
{
    public static TEnum ThrowIfNotDefined<TEnum>(
        this TEnum value,
        [CallerArgumentExpression(nameof(value))] string? paramName = default)
        where TEnum : struct, Enum
    {
        return Enum.IsDefined(value)
            ? value
            : throw new ArgumentOutOfRangeException(paramName, value, null);
    }
}