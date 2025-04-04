namespace GoAnyway.Framework.Json;

internal readonly record struct IndexOfResult(
    bool Found,
    long Index)
{
    public static IndexOfResult Success(long index)
    {
        return new(true, index);
    }

    public static IndexOfResult Fail()
    {
        return new(false, -1);
    }
}