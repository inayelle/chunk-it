using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Partitioners.Fixed;

public class FixedPartitioner : IPartitioner
{
    private readonly int _chunkSize;

    public int MinimumChunkSize => _chunkSize;
    public int MaximumChunkSize => _chunkSize;

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

    public override string ToString()
    {
        var builder = new DescriptionBuilder("fixed");

        return builder
            .AddParameter("size", _chunkSize)
            .Build();
    }
}