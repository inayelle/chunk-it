using ChunkIt.Common.Abstractions;
using ChunkIt.Metrics.Deduplication;
using ChunkIt.Metrics.Performance;

namespace ChunkIt.Metrics.Host;

public sealed class ChunkingReport
{
    public Input Input { get; }

    public PerformanceReport Performance { get; }
    public DeduplicationReport Deduplication { get; }

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
}