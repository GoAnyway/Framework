namespace GoAnyway.Framework.Memory;

public readonly struct Utf8Bytes
{
    private readonly Buffer<byte> _value;

    public int Length => _value.Length;

    public Utf8Bytes(Buffer<byte> value)
    {
        _value = value;
    }

    public static Utf8Bytes Sized(int length)
    {
        var buffer = Buffer<byte>.Sized(length);
        return new(buffer);
    }

    public void Dispose()
    {
        _value.Dispose();
    }

    public byte this[int index]
    {
        get => _value[index];
        set => _value[index] = value;
    }

    public Memory<byte> this[Range range] => _value[range];

    public static implicit operator Span<byte>(Utf8Bytes bytes)
    {
        return bytes._value;
    }

    public static implicit operator ReadOnlySpan<byte>(Utf8Bytes bytes)
    {
        return bytes._value;
    }

    public static implicit operator ReadOnlyMemory<byte>(Utf8Bytes bytes)
    {
        return bytes._value;
    }

    public static implicit operator Memory<byte>(Utf8Bytes bytes)
    {
        return bytes._value;
    }
}