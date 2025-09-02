using ChunkIt.Abstractions;

namespace ChunkIt.Sandbox;

internal sealed class ChunkingReport
{
    public required string FileName { get; init; }
    public required IPartitioner Partitioner { get; init; }
    public required IReadOnlyCollection<Chunk> Chunks { get; init; }

    public bool IsValid { get; set; }

    public int TotalChunks { get; set; }
    public int AverageChunkSize { get; set; }

    public int DistinctChunkHashes { get; set; }

    public int DuplicateChunkHashes { get; set; }
    public int DuplicateChunkEntries { get; set; }

    public long OriginalFileSize { get; set; }
    public long CompressedFileSize { get; set; }

    public long SavedBytes { get; set; }
    public float SavedRatio { get; set; }
}