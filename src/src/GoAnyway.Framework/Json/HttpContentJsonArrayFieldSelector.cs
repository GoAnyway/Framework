using System.Text.Json;

namespace GoAnyway.Framework.Json;

public sealed class HttpContentJsonArrayFieldSelector : IDisposable
{
    private readonly Stream _stream;
    private readonly string _arrayFieldName;
    private readonly Utf8JsonStreamFinder _finder;

    private HttpContentJsonArrayFieldSelector(
        Stream stream,
        string arrayFieldName)
    {
        _stream = stream;
        _arrayFieldName = arrayFieldName;
        _finder = new(stream);
    }

    public static async Task<HttpContentJsonArrayFieldSelector> CreateAsync(
        HttpContent content,
        string arrayFieldName,
        CancellationToken cancellationToken = default)
    {
        var stream = await Utf8JsonStreamEncoder.ReadContentStreamAsync(content, cancellationToken);

        return new(
            stream: stream, 
            arrayFieldName: arrayFieldName
        );
    }

    public async ValueTask<Stream> SelectAsync(CancellationToken cancellationToken = default)
    {
        if (!_stream.CanRead)
            throw new InvalidOperationException("Stream is already read.");

        var start = await FindStartIndexAsync(cancellationToken);
        var end = await FindEndIndexAsync(cancellationToken);

        return new ReadOnlyLimitedStream(_stream, start, end);
    }

    public void Dispose()
    {
        _finder.Dispose();
    }

    private async ValueTask<long> FindStartIndexAsync(CancellationToken cancellationToken = default)
    {
        var (found, _) = await _finder.IndexOfAsync(FieldPredicate, cancellationToken);
        if (!found)
            throw new InvalidOperationException($"Array field with provided name '{_arrayFieldName}' is not found.");

        (found, var start) = await _finder.IndexOfAsync(StartArrayPredicate, cancellationToken);
        if (!found)
            throw new InvalidOperationException($"Provided field with name '{_arrayFieldName}' is not an array.");

        return start;

        bool FieldPredicate(scoped Utf8JsonReader reader)
        {
            return reader.TokenType == JsonTokenType.PropertyName &&
                   reader.ValueTextEquals(_arrayFieldName);
        }

        bool StartArrayPredicate(scoped Utf8JsonReader reader)
        {
            return reader.TokenType == JsonTokenType.StartArray;
        }
    }

    private async ValueTask<long> FindEndIndexAsync(CancellationToken cancellationToken = default)
    {
        var arrayDepth = 1;
        var (found, end) = await _finder.IndexOfAsync(EndArrayPredicate, cancellationToken);

        if (!found || arrayDepth > 0)
            throw new ArgumentException("Invalid JSON format. Array field has no ending.");

        return end;

        bool EndArrayPredicate(scoped Utf8JsonReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.StartArray:
                    ++arrayDepth;
                    break;
                case JsonTokenType.EndArray when --arrayDepth == 0:
                    return true;
            }

            return false;
        }
    }
}