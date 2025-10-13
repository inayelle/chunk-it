using ChunkIt.Common.Abstractions;

namespace ChunkIt.Sandbox;

internal sealed class ChunkingReport
{
    public required SourceFile SourceFile { get; init; }
    public required IPartitioner Partitioner { get; init; }
    public required IReadOnlyCollection<Chunk> Chunks { get; init; }

    public TimeSpan Elapsed { get; set; }

    public int AverageChunkSize { get; set; }

    public long SavedBytes { get; set; }
    public float SavedRatio { get; set; }

    public long IndexBytes { get; set; }
    public float IndexRatio { get; set; }

    public float QualityRatio { get; set; }
}