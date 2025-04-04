using System.Text;

namespace GoAnyway.Framework.Json;

internal sealed class Utf8JsonStreamEncoder
{
    public static async Task<Stream> ReadContentStreamAsync(
        HttpContent content,
        CancellationToken cancellationToken = default)
    {
        var contentStream = await content.ReadAsStreamAsync(cancellationToken);

        // Wrap content stream into a transcoding stream that buffers the data transcoded from the sourceEncoding to utf-8.
        if (GetEncoding(content) is { } sourceEncoding &&
            !sourceEncoding.Equals(Encoding.UTF8))
        {
            contentStream = GetTranscodingStream(contentStream, sourceEncoding);
        }

        return contentStream;
    }

    private static Stream GetTranscodingStream(
        Stream contentStream,
        Encoding sourceEncoding)
    {
        return Encoding.CreateTranscodingStream(
            contentStream,
            sourceEncoding,
            Encoding.UTF8
        );
    }

    private static Encoding? GetEncoding(HttpContent content)
    {
        Encoding? encoding = null;

        if (content.Headers.ContentType?.CharSet is not { } charset)
            return encoding;

        try
        {
            // Remove at most a single set of quotes.
            if (charset.Length > 2 && charset[0] == '\"' && charset[^1] == '\"')
            {
                encoding = Encoding.GetEncoding(charset.Substring(1, charset.Length - 2));
            }
            else
            {
                encoding = Encoding.GetEncoding(charset);
            }
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException(message: null, ex);
        }

        return encoding;
    }
}