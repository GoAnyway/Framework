using GoAnyway.Framework.Assertion;
using GoAnyway.Framework.Extensions;

namespace GoAnyway.Framework.Configuration;

public static class EnvironmentVariable
{
    public static T Get<T>(
        string name,
        Func<string, T> converter)
    {
        converter.ThrowIfNull();
        return converter(Get(name));
    }

    public static T? Find<T>(
        string name,
        Func<string, T> converter)
    {
        converter.ThrowIfNull();

        var value = Find(name);
        if (value.IsNotSpecified())
            return default;

        return converter(value);
    }

    public static string Get(string name)
    {
        return Find(name) ?? 
               throw new InvalidOperationException($"Environment variable '{name}' is not set.");
    }

    public static string? Find(string name)
    {
        name.ThrowIfNullOrEmpty();
        return Environment.GetEnvironmentVariable(name);
    }
}