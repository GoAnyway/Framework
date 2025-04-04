using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using GoAnyway.Framework.Assertion;
using GoAnyway.Framework.Memory;

namespace GoAnyway.Framework.Extensions;

public static partial class StringExtensions
{
    [GeneratedRegex("^[a-z0-9_]+$", RegexOptions.Compiled)]
    private static partial Regex IsSnakeCaseRegex();

    [GeneratedRegex("([a-z0-9])([A-Z])", RegexOptions.Compiled)]
    private static partial Regex CamelToSnakeCaseRegex();

    public static bool IsSpecified([NotNullWhen(true)] this string? source)
    {
        return !string.IsNullOrEmpty(source);
    }

    public static bool IsNotSpecified([NotNullWhen(false)] this string? source)
    {
        return !IsSpecified(source);
    }

    public static Utf8Bytes ToUtf8Bytes(this string source)
    {
        source.ThrowIfNullOrEmpty();

        var encoding = Encoding.UTF8;
        var byteCount = encoding.GetByteCount(source);
        var buffer = Buffer<byte>.Sized(byteCount);
        encoding.GetBytes(source, buffer);
   
        return new(buffer);
    }

    public static bool IsSnakeCase(this string source)
    {
        source.ThrowIfNullOrEmpty();

        return IsSnakeCaseRegex().IsMatch(source);
    }

    public static string ToSnakeCase(this string source)
    {
        source.ThrowIfNullOrEmpty();

        const string format = "$1_$2";

        return CamelToSnakeCaseRegex()
            .Replace(source, format)
            .ToLower();
    }
}