using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using ChunkIt.Benchmarks.Extensions;

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
        var result = new ChunkingBenchmarkResult(summary, benchmarkCase);

        var baseline = summary
            .GetBenchmarkResults()
            .Where(static result => result.SourceFile.Equals(result.SourceFile))
            .MaxBy(static result => result.Throughput);

        if (result.Equals(baseline))
        {
            return "1.00";
        }

        var ratio = result.Throughput / baseline.Throughput;

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