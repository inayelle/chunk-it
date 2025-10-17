using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Metrics.Performance.Extensions;

internal static class BenchmarkExtensions
{
    public static IEnumerable<(Input Input, PerformanceReport Report)> GetPerformanceReports(
        this Summary summary
    )
    {
        foreach (var benchmark in summary.BenchmarksCases)
        {
            var input = benchmark.GetInput();

            var statistics = summary[benchmark]!.ResultStatistics;

            var report = new PerformanceReport(input.SourceFile, statistics);

            yield return (input, report);
        }
    }

    public static Input GetInput(this BenchmarkCase benchmarkCase)
    {
        return benchmarkCase
            .Parameters
            .Items
            .Select(item => item.Value)
            .OfType<Input>()
            .Single();
    }
}