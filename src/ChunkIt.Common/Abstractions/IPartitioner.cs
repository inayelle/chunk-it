namespace ChunkIt.Common.Abstractions;

public interface IPartitioner
{
    int MinimumChunkSize { get; }
    int MaximumChunkSize { get; }

    int FindChunkLength(ReadOnlySpan<byte> buffer);
}