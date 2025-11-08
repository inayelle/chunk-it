using ChunkIt.Common.Abstractions;
using ChunkIt.Metrics.Deduplication;
using ChunkIt.Metrics.Performance;
using UnitsNet;

namespace ChunkIt.Metrics.Host;

public sealed class ChunkingReport : IComparable<ChunkingReport>
{
    public Input Input { get; }

    public PerformanceReport Performance { get; }
    public DeduplicationReport Deduplication { get; }

    public BitRate SavedBytesThroughput { get; }

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

        SavedBytesThroughput = BitRate.FromBytesPerSecond(
            deduplicationReport.SavedBytes / performanceReport.Mean.Seconds
        );
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