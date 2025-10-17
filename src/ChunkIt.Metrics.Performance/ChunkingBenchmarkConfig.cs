using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using ChunkIt.Metrics.Performance.Exporters;
using ChunkIt.Metrics.Performance.Throughput;
using Perfolizer.Horology;

namespace ChunkIt.Metrics.Performance;

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
            .WithMinIterationTime(TimeInterval.FromMilliseconds(300))
            .WithUnrollFactor(1)
        );

        AddExporter(new ScottPlotMeanExporter());
        AddExporter(new ScottPlotThroughputExporter());
    }
}