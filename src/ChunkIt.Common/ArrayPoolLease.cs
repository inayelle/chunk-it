using System.Buffers;

namespace ChunkIt.Common;

public readonly struct ArrayPoolLease<T> : IDisposable
{
    private readonly T[] _data;
    private readonly int _length;

    public Memory<T> Memory => _data.AsMemory(start: 0, length: _length);
    public Span<T> Span => _data.AsSpan(start: 0, length: _length);

    public ArrayPoolLease(int length)
    {
        _length = length;

        _data = ArrayPool<T>.Shared.Rent(length);
    }

    public void Dispose()
    {
        ArrayPool<T>.Shared.Return(_data);
    }
}