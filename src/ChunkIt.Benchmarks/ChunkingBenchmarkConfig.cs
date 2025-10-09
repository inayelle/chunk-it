using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
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

        AddDiagnoser(new MemoryDiagnoser(new MemoryDiagnoserConfig(displayGenColumns: true)));

        AddJob(Job
            .Default
            .WithGcServer(false)
            .WithGcConcurrent(false)
            .WithMinIterationCount(25)
            .WithInvocationCount(1)
            .WithUnrollFactor(1)
        );
    }
}