namespace ChunkIt.Common.Abstractions;

public interface IPartitioner
{
    int MinimumChunkSize { get; }
    int AverageChunkSize { get; }
    int MaximumChunkSize { get; }

    int FindChunkLength(ReadOnlySpan<byte> buffer);
}