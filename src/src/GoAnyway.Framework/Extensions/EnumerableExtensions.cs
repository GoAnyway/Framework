using System.Collections;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using GoAnyway.Framework.Assertion;

namespace GoAnyway.Framework.Extensions;

public static class EnumerableExtensions
{
    public static IReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> source)
    {
        source.ThrowIfNull();

        return source
            .ToImmutableArray()
            .AsReadOnly();
    }

    public static IReadOnlyCollection<T> ToReadOnly<T>(this ICollection<T> source)
    {
        source.ThrowIfNull();
        return new ReadOnlyCollectionAdapter<T>(source);
    }

    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
    {
        source.ThrowIfNull();
        return new(source);
    }

    private sealed class ReadOnlyCollectionAdapter<T> : IReadOnlyCollection<T>
    {
        private readonly ICollection<T> _source;

        public int Count => _source.Count;

        public ReadOnlyCollectionAdapter(ICollection<T> source)
        {
            _source = source.ThrowIfNull();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}