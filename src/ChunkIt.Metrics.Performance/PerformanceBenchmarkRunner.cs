using BenchmarkDotNet.Running;
using ChunkIt.Common.Abstractions;
using ChunkIt.Metrics.Performance.Extensions;

namespace ChunkIt.Metrics.Performance;

public sealed class PerformanceBenchmarkRunner
{
    public IEnumerable<(Input Input, PerformanceReport Report)> Run()
    {
        var summary = BenchmarkRunner.Run<PerformanceBenchmark>();

        return summary.GetPerformanceReports();
    }
}