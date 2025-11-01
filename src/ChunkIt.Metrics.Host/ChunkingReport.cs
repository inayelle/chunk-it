using ChunkIt.Common.Abstractions;
using ChunkIt.Metrics.Deduplication;
using ChunkIt.Metrics.Performance;

namespace ChunkIt.Metrics.Host;

public sealed class ChunkingReport : IComparable<ChunkingReport>
{
    public Input Input { get; }

    public PerformanceReport Performance { get; }
    public DeduplicationReport Deduplication { get; }

    public IReadOnlyList<Chunk> Chunks => Deduplication.Chunks;

    public ChunkingReport(
        Input input,
        PerformanceReport performanceReport,
        DeduplicationReport deduplicationReport
    )
    {
        Input = input;
        Performance = performanceReport;
        Deduplication = deduplicationReport;
    }

    public int CompareTo(ChunkingReport other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        return Input.CompareTo(other.Input);
    }
}