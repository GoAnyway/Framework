using System.Diagnostics;
using System.Runtime.CompilerServices;
using GoAnyway.Framework.Assertion;

namespace GoAnyway.Framework.Extensions;

public static class AsyncEnumerableExtensions
{
    public static async Task<IReadOnlyCollection<T>> ToReadOnlyAsync<T>(
        this IAsyncEnumerable<T> source,
        CancellationToken cancellationToken = default)
    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        source.ThrowIfNull();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        var list = new List<T>();
        await foreach (var item in source.WithCancellation(cancellationToken))
        {
            list.Add(item);
        }

        return list;
    }

    public static IAsyncEnumerable<T[]> Chunk<T>(
        this IAsyncEnumerable<T> source,
        int size,
        CancellationToken cancellationToken = default)
    {
        source.ThrowIfNull();
        return ChunkIterator(source, size, cancellationToken);
    }

    private static async IAsyncEnumerable<T[]> ChunkIterator<T>(
        IAsyncEnumerable<T> source,
        int size,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await using var e = source.GetAsyncEnumerator(cancellationToken);

        // Before allocating anything, make sure there's at least one element.
        if (!await e.MoveNextAsync())
            yield break;

        // Now that we know we have at least one item, allocate an initial storage array. This is not
        // the array we'll yield.  It starts out small in order to avoid significantly overallocating
        // when the source has many fewer elements than the chunk size.
        var arraySize = Math.Min(size, 4);
        int i;
        do
        {
            var array = new T[arraySize];

            // Store the first item.
            array[0] = e.Current;
            i = 1;

            if (size != array.Length)
            {
                // This is the first chunk. As we fill the array, grow it as needed.
                for (; i < size && await e.MoveNextAsync(); i++)
                {
                    if (i >= array.Length)
                    {
                        arraySize = (int)Math.Min((uint)size, 2 * (uint)array.Length);
                        Array.Resize(ref array, arraySize);
                    }

                    array[i] = e.Current;
                }
            }
            else
            {
                // For all but the first chunk, the array will already be correctly sized.
                // We can just store into it until either it's full or MoveNext returns false.
                var local = array; // avoid bounds checks by using cached local (`array` is lifted to iterator object as a field)
                Debug.Assert(local.Length == size);
                for (; (uint)i < (uint)local.Length && await e.MoveNextAsync(); i++)
                {
                    local[i] = e.Current;
                }
            }

            if (i != array.Length)
            {
                Array.Resize(ref array, i);
            }

            yield return array;
        } while (i >= size && await e.MoveNextAsync());
    }
}