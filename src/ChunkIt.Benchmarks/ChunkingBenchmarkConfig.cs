using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using ChunkIt.Benchmarks.Throughput;

namespace ChunkIt.Benchmarks;

public sealed class ChunkingBenchmarkConfig : ManualConfig
{
    public ChunkingBenchmarkConfig()
    {
        SummaryStyle = SummaryStyle.Default.WithMaxParameterColumnWidth(64);

        WithOptions(ConfigOptions.DisableLogFile);
        AddColumn(new ThroughputColumn());
        AddColumn(new ThroughputRatioColumn());
        WithOrderer(new ThroughputOrderer());
    }
}