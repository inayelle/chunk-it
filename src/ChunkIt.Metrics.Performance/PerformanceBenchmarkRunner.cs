using BenchmarkDotNet.Running;
using ChunkIt.Common.Abstractions;
using ChunkIt.Metrics.Performance.Extensions;

namespace ChunkIt.Metrics.Performance;

public sealed class PerformanceBenchmarkRunner : IBenchmarkRunner<PerformanceReport>
{
    public Task<Dictionary<Input, PerformanceReport>> Run()
    {
        var summary = BenchmarkRunner.Run<PerformanceBenchmark>();

        var reports = summary
            .GetPerformanceReports()
            .ToDictionary(entry => entry.Input, entry => entry.Report);

        return Task.FromResult(reports);
    }
}