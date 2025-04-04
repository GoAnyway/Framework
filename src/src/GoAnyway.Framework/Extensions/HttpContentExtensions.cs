using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using GoAnyway.Framework.Json;

namespace GoAnyway.Framework.Extensions;

public static class HttpContentExtensions
{
    public static async IAsyncEnumerable<TArrayElement> ReadFromJsonAsAsyncEnumerable<TValue, TArrayElement>(
        this HttpContent content,
        Expression<Func<TValue, IEnumerable<TArrayElement>>> arrayFieldSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var arrayFieldName = arrayFieldSelector.GetMemberName();
        var contentArrayFieldSelector = await HttpContentJsonArrayFieldSelector.CreateAsync(
            content, 
            arrayFieldName,
            cancellationToken
        );

        var arrayStream = await contentArrayFieldSelector.SelectAsync(cancellationToken);
        var limitedContent = new StreamContent(arrayStream);

        var elements = limitedContent.ReadFromJsonAsAsyncEnumerable<TArrayElement>(cancellationToken);
        await foreach (var element in elements)
        {
            yield return element!;
        }
    }
}