using GoAnyway.Framework.Assertion;

namespace GoAnyway.Framework.Extensions;

public static class ByteExtensions
{
    public static string ToBase64String(this byte[] source)
    {
        source.ThrowIfNull();
        return source.AsSpan().ToBase64String();
    }

    public static string ToBase64String(this Memory<byte> source)
    {
        return source.Span.ToBase64String();
    }

    public static string ToBase64String(this ReadOnlyMemory<byte> source)
    {
        return source.Span.ToBase64String();
    }

    public static string ToBase64String(this Span<byte> source)
    {
        return ((ReadOnlySpan<byte>)source).ToBase64String();
    }

    public static string ToBase64String(this ReadOnlySpan<byte> source)
    {
        return Convert.ToBase64String(source);
    }
}