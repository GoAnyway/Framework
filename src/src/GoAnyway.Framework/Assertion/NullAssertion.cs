using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace GoAnyway.Framework.Assertion;

public static class NullAssertion
{
    // TODO: Fix warning with nullable return.
    [return: System.Diagnostics.CodeAnalysis.NotNull]
    public static T ThrowIfNull<T>(
        [NoEnumeration] this T? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = default)
        where T : class?
    {
        ArgumentNullException.ThrowIfNull(value, paramName);
        return value;
    }

    public static T ThrowIfNull<T>(
        this T? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = default)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(value, paramName);
        return value.Value;
    }
}