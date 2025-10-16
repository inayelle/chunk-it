using ChunkIt.Common.Abstractions;

namespace ChunkIt.Partitioning.AsymmetricExtremum;

public sealed partial class AsymmetricExtremumPartitioner
    : IPartitioner,
      IEquatable<AsymmetricExtremumPartitioner>
{
    private readonly int _windowSize;

    public int MinimumChunkSize { get; }
    public int AverageChunkSize { get; }
    public int MaximumChunkSize { get; }

    public AsymmetricExtremumPartitioner(
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

        var max = (Value: buffer[0], Offset: 0);
        var cursor = 1;

        while (cursor < buffer.Length)
        {
            if (buffer[cursor] > max.Value)
            {
                max = (Value: buffer[cursor], Offset: cursor);
            }
            else if (cursor >= MinimumChunkSize && cursor == max.Offset + _windowSize)
            {
                return cursor;
            }
            else
            {
                cursor += 1;
            }
        }

        return buffer.Length;
    }
}