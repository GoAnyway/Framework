using System.Collections.Concurrent;
using GoAnyway.Framework.Assertion;

namespace GoAnyway.Framework.Conversion;

public static class EnumConverter
{
    private static readonly ConcurrentDictionary<EnumConversionCacheKey, Enum> Cache = new();

    public static TTo ConvertTo<TTo>(this Enum value)
        where TTo : struct, Enum
    {
        value.ThrowIfNull();

        var key = new EnumConversionCacheKey(
            value.GetType(), 
            typeof(TTo), 
            value.ToString()
        );

        if (Cache.TryGetValue(key, out var cached))
            return (TTo)cached;

        if (Enum.TryParse<TTo>(value.ToString(), out var result))
        {
            Cache[key] = result;
            return result;
        }

        throw new ArgumentException($"Cannot convert '{value}' " +
                                    $"from {value.GetType().Name} " +
                                    $"to {typeof(TTo).Name}");
    }

    private readonly record struct EnumConversionCacheKey(
        Type SourceType,
        Type DestinationType,
        string Value
    );
}