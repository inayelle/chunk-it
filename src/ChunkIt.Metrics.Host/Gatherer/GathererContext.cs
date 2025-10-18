using ChunkIt.Common.Abstractions;
using ChunkIt.Metrics.Deduplication;
using ChunkIt.Metrics.Performance;

namespace ChunkIt.Metrics.Host.Gatherer;

internal sealed class GathererContext
{
    private readonly Dictionary<Input, PerformanceReport> _performanceReports = new();
    private readonly Dictionary<Input, DeduplicationReport> _deduplicationReports = new();

    public void AddReport(Input input, PerformanceReport report)
    {
        _performanceReports.Add(input, report);
    }

    public void AddReport(Input input, DeduplicationReport report)
    {
        _deduplicationReports.Add(input, report);
    }

    public ChunkingReport GatherReport(Input input)
    {
        var performanceReport = _performanceReports[input];
        var deduplicationReport = _deduplicationReports[input];

        return new ChunkingReport(input, performanceReport, deduplicationReport);
    }
}