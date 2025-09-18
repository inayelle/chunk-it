using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace ChunkIt.Benchmarks.Throughput;

internal static class ThroughputExtensions
{
    private static readonly string[] Units =
    [
        "b",
        "Kb",
        "Mb",
        "Gb",
        "Tb",
        "Eb",
    ];

    public static SourceFile GetSourceFile(this BenchmarkCase benchmarkCase)
    {
        return benchmarkCase
            .Parameters
            .Items
            .Select(item => item.Value)
            .OfType<SourceFile>()
            .FirstOrDefault();
    }

    public static double GetThroughput(this Summary summary, BenchmarkCase benchmarkCase)
    {
        var sourceFile = benchmarkCase.GetSourceFile();

        if (sourceFile is null)
        {
            return Double.NaN;
        }

        var stats = summary[benchmarkCase]!.ResultStatistics;

        if (stats is null)
        {
            return Double.NaN;
        }

        var throughput = sourceFile.Size / stats.Mean;

        return throughput;
    }

    public static string GetThroughputText(this Summary summary, BenchmarkCase benchmarkCase)
    {
        var throughput = summary.GetThroughput(benchmarkCase) * 1e9 * 8;

        var index = 0;
        while (throughput > 1024.0d)
        {
            throughput /= 1024.0d;
            index += 1;
        }

        var unit = Units[index];

        throughput = Math.Round(throughput, 2);

        return $"{throughput} {unit}/s";
    }
}