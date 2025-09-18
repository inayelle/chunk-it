using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace ChunkIt.Benchmarks.Throughput;

internal sealed class ThroughputRatioColumn : IColumn
{
    public string Id => "ThroughputRatio";
    public string ColumnName => "Throughput ratio";
    public string Legend => "ThroughputRatio";

    public bool AlwaysShow => true;
    public bool IsNumeric => true;

    public ColumnCategory Category => ColumnCategory.Custom;
    public UnitType UnitType => UnitType.Dimensionless;

    public int PriorityInCategory => 0;

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        var currentSourceFile = benchmarkCase.GetSourceFile();

        var baseline = summary
            .BenchmarksCases
            .Where(benchmark => benchmark.GetSourceFile().Equals(currentSourceFile))
            .MaxBy(summary.GetThroughput);

        if (baseline == benchmarkCase)
        {
            return "1.00";
        }

        var baselineThroughput = summary.GetThroughput(baseline);
        var currentThroughput = summary.GetThroughput(benchmarkCase);

        var ratio = currentThroughput / baselineThroughput;

        return $"{ratio:F2}";
    }

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
    {
        return GetValue(summary, benchmarkCase);
    }

    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase)
    {
        return true;
    }

    public bool IsAvailable(Summary summary)
    {
        return true;
    }
}