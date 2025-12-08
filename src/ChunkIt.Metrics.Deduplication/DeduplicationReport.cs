using ChunkIt.Common.Abstractions;

namespace ChunkIt.Metrics.Deduplication;

public sealed class DeduplicationReport
{
    public IReadOnlyList<Chunk> Chunks { get; }

    public int AverageChunkSize { get; set; }

    public long SavedBytes { get; set; }
    public float SavedRatio { get; set; }

    public float VarianceRatio { get; set; }
    public float QualityRatio { get; set; }

    public DeduplicationReport(IReadOnlyList<Chunk> chunks)
    {
        Chunks = chunks;
    }
}