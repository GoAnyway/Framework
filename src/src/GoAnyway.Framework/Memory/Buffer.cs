using System.Buffers;

namespace GoAnyway.Framework.Memory;

public readonly struct Buffer<T>
{
    private readonly T[] _array;
    private readonly bool _rented;

    public Memory<T> Value { get; }
    public int Length => Value.Length;

    private Buffer(
        T[] array, 
        bool rented,
        Memory<T> value)
    {
        _array = array;
        _rented = rented;
        Value = value;
    }

    public static Buffer<T> Sized(int length)
    {
        var array = ArrayPool<T>.Shared.Rent(length);

        return new(
            array: array,
            rented: true,
            value: array.AsMemory(..length)
        );
    }

    //public static Buffer<T> Wrap(Memory<T> memory)
    //{
    //    return new(
    //        array: [],
    //        rented: false,
    //        memory: memory
    //    );
    //}

    public static bool CanUseStackalloc(int size)
    {
        const int stackallocThreeshold = 1024;
        return size <= stackallocThreeshold;
    }

    public void Dispose()
    {
        if (!_rented)
            return;

        ArrayPool<T>.Shared.Return(_array);
    }

    //public T[] ToArray()
    //{
    //    return _rented ? _array : Value.ToArray();
    //}

    public T this[int index]
    {
        get => Value.Span[index];
        set => Value.Span[index] = value;
    }

    public Memory<T> this[Range range] => Value[range];

    public static implicit operator Span<T>(Buffer<T> buffer)
    {
        return buffer.Value.Span;
    }

    public static implicit operator ReadOnlySpan<T>(Buffer<T> buffer)
    {
        return buffer.Value.Span;
    }

    public static implicit operator ReadOnlyMemory<T>(Buffer<T> buffer)
    {
        return buffer.Value;
    }

    public static implicit operator Memory<T>(Buffer<T> buffer)
    {
        return buffer.Value;
    }
}