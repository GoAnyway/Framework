using System.Text.Json;
using GoAnyway.Framework.Memory;

namespace GoAnyway.Framework.Json;

internal sealed class Utf8JsonStreamFinder
{
    private readonly Stream _stream;
    private readonly Utf8Bytes _buffer;

    private JsonReaderState _readerState;
    private int _bufferOffset;

    public delegate bool JsonPredicate(scoped Utf8JsonReader reader);

    public Utf8JsonStreamFinder(Stream stream)
    {
        _stream = stream;
        _buffer = Utf8Bytes.Sized(4096);
    }

    public async ValueTask<IndexOfResult> IndexOfAsync(
        JsonPredicate predicate,
        CancellationToken cancellationToken = default)
    {
        if (_bufferOffset == _buffer.Length)
            _bufferOffset = 0;

        var read = _bufferOffset > 0
            ? _buffer.Length
            : await _stream.ReadAsync(_buffer, cancellationToken);

        while (read > 0)
        {
            var buffer = _buffer[_bufferOffset..read];
            var index = IndexOfCore(buffer, predicate);

            if (index > 0)
                return IndexOfResult.Success(index);

            read = _bufferOffset > 0
                ? _buffer.Length
                : await _stream.ReadAsync(_buffer, cancellationToken);
        }

        return IndexOfResult.Fail();
    }

    public void Dispose()
    {
        _buffer.Dispose();
    }

    private long IndexOfCore(
        Memory<byte> buffer,
        JsonPredicate predicate)
    {
        var reader = new Utf8JsonReader(buffer.Span, false, _readerState);
        while (reader.Read())
        {
            _readerState = reader.CurrentState;

            if (predicate(reader))
            {
                _bufferOffset += (int)reader.BytesConsumed;
                return reader.TokenStartIndex;
            }
        }

        _bufferOffset = 0;
        return -1;
    }
}