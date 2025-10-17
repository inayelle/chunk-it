using System.Collections.Immutable;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace ChunkIt.Benchmarks.Throughput;

internal sealed class ThroughputOrderer : DefaultOrderer
{
    public override IEnumerable<BenchmarkCase> GetSummaryOrder(
        ImmutableArray<BenchmarkCase> benchmarksCases,
        Summary summary
    )
    {
        return benchmarksCases
            .Select(benchmark => new ChunkingBenchmarkResult(summary, benchmark))
            .OrderBy(result => result.SourceFile.Size)
            .ThenByDescending(result => result.Throughput)
            .Select(result => result.Benchmark);
    }
}