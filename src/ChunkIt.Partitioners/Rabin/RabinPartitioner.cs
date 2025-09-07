using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Partitioners.Rabin;

public class RabinPartitioner : IPartitioner
{
    private const ulong Base = 257UL;

    private readonly int _windowSize;
    private readonly ulong _mask;
    private readonly ulong _baseValue;

    public int MinimumChunkSize { get; }
    public int AverageChunkSize { get; }
    public int MaximumChunkSize { get; }

    public RabinPartitioner(
        int minimumChunkSize,
        int averageChunkSize,
        int maximumChunkSize,
        int windowSize
    )
    {
        MinimumChunkSize = minimumChunkSize;
        AverageChunkSize = averageChunkSize;
        MaximumChunkSize = maximumChunkSize;

        _windowSize = windowSize;

        _mask = CalculateMask(averageChunkSize);
        _baseValue = CalculateBaseValue(windowSize);
    }

    public int FindChunkLength(ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length <= MinimumChunkSize || buffer.Length <= _windowSize)
        {
            return buffer.Length;
        }

        if (buffer.Length > MaximumChunkSize)
        {
            buffer = buffer.Slice(start: 0, length: MaximumChunkSize);
        }

        var hash = 0UL;
        var cursor = 0;

        for (; cursor < _windowSize; cursor += 1)
        {
            hash = unchecked(hash * Base + buffer[cursor]);
        }

        for (; cursor < buffer.Length; cursor += 1)
        {
            if ((hash & _mask) == 0 && cursor >= MinimumChunkSize)
            {
                return cursor;
            }

            hash = unchecked((hash - _baseValue * buffer[cursor - _windowSize]) * Base + buffer[cursor]);
        }

        return buffer.Length;
    }

    private static ulong CalculateMask(int averageChunkSize)
    {
        var k = (int)Math.Ceiling(Math.Log2(averageChunkSize));

        var kMask = Math.Min(63, k);

        return (1UL << kMask) - 1;
    }

    private static ulong CalculateBaseValue(int windowSize)
    {
        var baseValue = 1UL;

        for (var i = 0; i < windowSize - 1; i++)
        {
            baseValue = unchecked(baseValue * Base);
        }

        return baseValue;
    }

    public override string ToString()
    {
        var builder = new DescriptionBuilder("rabin");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("window", _windowSize)
            .Build();
    }
}