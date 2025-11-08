using ChunkIt.Common.Abstractions;

namespace ChunkIt.Partitioning.Fixed;

public sealed partial class FixedPartitioner
    : IPartitioner,
      IEquatable<FixedPartitioner>
{
    private readonly int _chunkSize;

    public int MinimumChunkSize => _chunkSize;
    public int AverageChunkSize => _chunkSize;
    public int MaximumChunkSize => _chunkSize;

    public string Name => "fixed";

    public FixedPartitioner(int chunkSize)
    {
        _chunkSize = chunkSize;
    }

    public int FindChunkLength(ReadOnlySpan<byte> buffer)
    {
        return buffer.Length < _chunkSize
            ? buffer.Length
            : _chunkSize;
    }
}