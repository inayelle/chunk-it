using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Partitioners.Ram;

public class RamPartitioner : IPartitioner
{
    private readonly int _windowSize;

    public int MinimumChunkSize { get; }
    public int MaximumChunkSize { get; }

    public RamPartitioner(
        int minimumChunkSize,
        int maximumChunkSize,
        int windowSize
    )
    {
        MinimumChunkSize = minimumChunkSize;
        MaximumChunkSize = maximumChunkSize;
        _windowSize = windowSize;
    }

    public int FindChunkLength(ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length <= MinimumChunkSize)
        {
            return buffer.Length;
        }

        if (buffer.Length > MaximumChunkSize)
        {
            buffer = buffer.Slice(start: 0, length: MaximumChunkSize);
        }

        var maxValue = Byte.MinValue;
        var windowSize = Math.Min(_windowSize, buffer.Length);
        var cursor = 0;

        for (; cursor < windowSize; cursor++)
        {
            if (buffer[cursor] > maxValue)
            {
                maxValue = buffer[cursor];
            }
        }

        for (; cursor < buffer.Length; cursor++)
        {
            if (buffer[cursor] >= maxValue)
            {
                return cursor;
            }
        }

        return cursor;
    }

    public override string ToString()
    {
        var builder = new DescriptionBuilder("ram");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("window", _windowSize)
            .Build();
    }
}