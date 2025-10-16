using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Partitioning.RapidAsymmetricMaximum;

public sealed class RapidAsymmetricMaximumPartitioner : IPartitioner
{
    private readonly int _windowSize;

    public int MinimumChunkSize { get; }
    public int AverageChunkSize { get; }
    public int MaximumChunkSize { get; }

    public RapidAsymmetricMaximumPartitioner(
        int minimumChunkSize,
        int averageChunkSize,
        int maximumChunkSize
    )
    {
        MinimumChunkSize = minimumChunkSize;
        AverageChunkSize = averageChunkSize;
        MaximumChunkSize = maximumChunkSize;

        _windowSize = averageChunkSize - 256;
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
        var builder = new DescriptionBuilder("rapid-asymmetric-maximum");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("window", _windowSize)
            .Build();
    }
}