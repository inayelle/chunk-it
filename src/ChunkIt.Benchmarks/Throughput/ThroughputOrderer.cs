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
            .OrderBy(benchmark => benchmark.GetSourceFile().Size)
            .ThenByDescending(summary.GetThroughput);
    }
}