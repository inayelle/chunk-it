namespace ChunkIt.Common.Abstractions;

public interface IPartitioner
{
    int MinimumChunkSize { get; }
    int AverageChunkSize { get; }
    int MaximumChunkSize { get; }

    string Name { get; }

    int FindChunkLength(ReadOnlySpan<byte> buffer);
}