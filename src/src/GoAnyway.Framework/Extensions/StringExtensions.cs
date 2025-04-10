using System.Diagnostics.CodeAnalysis;
using System.Text;
using GoAnyway.Framework.Assertion;
using GoAnyway.Framework.Memory;

namespace GoAnyway.Framework.Extensions;

public static partial class StringExtensions
{
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

    public static bool IsSnakeCase(this string? input)
    {
        if (input.IsNotSpecified())
            return false;

        if (input.StartsWith('_') || 
            input.EndsWith('_') ||
            input.Contains("__"))
        {
            return false;
        }

        return input.All(c => char.IsLower(c) || 
                              char.IsDigit(c) || 
                              c == '_');
    }

    [return: NotNullIfNotNull(nameof(input))]
    public static string? ToSnakeCase(this string? input)
    {
        if (input.IsNotSpecified())
            return input;

        var sb = new StringBuilder();

        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];

            if (char.IsUpper(c))
            {
                var isPrevLower = i > 0 && char.IsLower(input[i - 1]);
                var isNextLower = i + 1 < input.Length && char.IsLower(input[i + 1]);

                if (i > 0 && (isPrevLower || isNextLower))
                    sb.Append('_');

                sb.Append(char.ToLower(c));
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}