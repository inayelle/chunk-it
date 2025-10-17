using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using UnitsNet.Units;

namespace ChunkIt.Benchmarks.Throughput;

internal sealed class ThroughputColumn : IColumn
{
    public string Id => "Throughput";
    public string ColumnName => "Throughput";
    public string Legend => "Throughput";

    public bool AlwaysShow => true;
    public bool IsNumeric => true;

    public ColumnCategory Category => ColumnCategory.Custom;
    public UnitType UnitType => UnitType.Dimensionless;

    public int PriorityInCategory => 0;

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        var result = new ChunkingBenchmarkResult(summary, benchmarkCase);

        return result.Throughput.ToUnit(BitRateUnit.GigabitPerSecond).ToString();
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