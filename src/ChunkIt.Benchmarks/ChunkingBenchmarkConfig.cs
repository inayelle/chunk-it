using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;

namespace ChunkIt.Benchmarks;

public sealed class ChunkingBenchmarkConfig : ManualConfig
{
    public ChunkingBenchmarkConfig()
    {
        SummaryStyle = SummaryStyle.Default.WithMaxParameterColumnWidth(64);

        WithOptions(ConfigOptions.DisableLogFile);
    }
}