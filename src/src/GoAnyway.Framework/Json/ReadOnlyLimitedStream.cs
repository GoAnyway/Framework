namespace GoAnyway.Framework.Json;

internal sealed class ReadOnlyLimitedStream : Stream
{
    private readonly Stream _innerStream;
    private readonly long _start;
    private readonly long _end;

    public ReadOnlyLimitedStream(
        Stream innerStream,
        long start,
        long end)
    {
        ValidateInnerStream(innerStream);

        _innerStream = innerStream;
        _start = start;
        _end = end;

        _innerStream.Seek(start, SeekOrigin.Begin);
    }

    public override long Length => _innerStream.Length;

    public override long Position
    {
        get => _innerStream.Position;
        set => _innerStream.Position = value;
    }

    public override bool CanRead => _innerStream.CanRead;

    public override bool CanSeek => _innerStream.CanSeek;

    public override bool CanWrite => false;

    public override async Task<int> ReadAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken)
    {
        return await ReadAsync(new(buffer, offset, count), cancellationToken);
    }

    public override async ValueTask<int> ReadAsync(
        Memory<byte> buffer,
        CancellationToken cancellationToken = default)
    {
        var remainingLength = _end - Position;
        if (remainingLength <= 0)
            return 0;

        if (buffer.Length > remainingLength)
            buffer = buffer[..(int)remainingLength];

        return await _innerStream.ReadAsync(buffer, cancellationToken);
    }

    public override int Read(
        byte[] buffer,
        int offset,
        int count)
    {
        var remainingLength = _end - Position;
        if (remainingLength <= 0)
            return 0;

        if (count > remainingLength)
            count = (int)remainingLength;

        return _innerStream.Read(buffer, offset, count);
    }

    public override void Flush()
    {
        _innerStream.Flush();
    }

    public override Task FlushAsync(CancellationToken cancellationToken)
    {
        return _innerStream.FlushAsync(cancellationToken);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        if (offset < _start)
            offset = _start;

        return _innerStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        if (value > _end)
            value = _end;

        _innerStream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }

    private static void ValidateInnerStream(Stream innerStream)
    {
        if (!innerStream.CanRead)
            throw new NotSupportedException();

        if (!innerStream.CanSeek)
            throw new NotSupportedException();
    }
}