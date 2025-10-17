using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using Perfolizer.Horology;

namespace ChunkIt.Metrics.Performance;

public sealed class PerformanceBenchmarkConfig : ManualConfig
{
    public PerformanceBenchmarkConfig()
    {
        SummaryStyle = SummaryStyle.Default.WithMaxParameterColumnWidth(64);

        WithOptions(ConfigOptions.DisableLogFile);

        AddJob(Job
            .Default
            .WithGcServer(false)
            .WithGcConcurrent(false)
            .WithMinIterationTime(TimeInterval.FromMilliseconds(300))
            .WithUnrollFactor(1)
        );
    }
}