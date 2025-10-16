using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Benchmarks.Extensions;

internal static class BenchmarkExtensions
{
    public static SourceFile GetSourceFile(this BenchmarkCase benchmarkCase)
    {
        return benchmarkCase
            .Parameters
            .Items
            .Select(item => item.Value)
            .OfType<SourceFile>()
            .Single();
    }

    public static IPartitioner GetPartitioner(this BenchmarkCase benchmarkCase)
    {
        return benchmarkCase
            .Parameters
            .Items
            .Select(item => item.Value)
            .OfType<IPartitioner>()
            .Single();
    }

    public static ChunkingBenchmarkResult[] GetBenchmarkResults(this Summary summary)
    {
        return summary
            .BenchmarksCases
            .Select(benchmark => ChunkingBenchmarkResult.FromBenchmark(benchmark, summary))
            .OrderBy(entry => entry.SourceFile.Name)
            .ThenBy(entry => entry.Throughput)
            .ToArray();
    }

    public static ChunkingBenchmarkResult.SourceFileGroup[] GetBenchmarkGroups(this Summary summary)
    {
        return summary
            .BenchmarksCases
            .Select(benchmark => ChunkingBenchmarkResult.FromBenchmark(benchmark, summary))
            .OrderBy(result => result.SourceFile.Name)
            .ThenBy(result => result.Throughput)
            .GroupBy(
                entry => entry.SourceFile,
                (sourceFile, entries) => new ChunkingBenchmarkResult.SourceFileGroup
                {
                    SourceFile = sourceFile,
                    Results = entries.ToArray(),
                }
            )
            .ToArray();
    }
}