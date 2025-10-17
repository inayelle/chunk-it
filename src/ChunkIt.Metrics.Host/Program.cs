using ChunkIt.Metrics.Deduplication;
using ChunkIt.Metrics.Host;
using ChunkIt.Metrics.Inputs;
using ChunkIt.Metrics.Performance;
using UnitsNet.Units;

var performanceBenchmarkRunner = new PerformanceBenchmarkRunner();
var performanceReports = await performanceBenchmarkRunner.Run();

var deduplicationBenchmarkRunner = new DeduplicationBenchmarkRunner();
var deduplicationReports = await deduplicationBenchmarkRunner.Run();

var chunkingReports = InputsProvider
    .Enumerate()
    .Select(input => new ChunkingReport(
            input,
            performanceReports[input],
            deduplicationReports[input]
        )
    )
    .OrderBy(report => report.Input.Index)
    .ToArray();

foreach (var chunkingReport in chunkingReports)
{
    Console.WriteLine($"{chunkingReport.Input}");
    Console.WriteLine($" - mean time: {chunkingReport.Performance.Mean.ToUnit(DurationUnit.Millisecond)}");
    Console.WriteLine($" - throughput: {chunkingReport.Performance.Throughput.ToUnit(BitRateUnit.GigabitPerSecond)}");
    Console.WriteLine($" - dedup ratio: {chunkingReport.Deduplication.SavedRatio}");
    Console.WriteLine($" - quality ratio: {chunkingReport.Deduplication.QualityRatio}");
}