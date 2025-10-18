using ChunkIt.Metrics.Host.Gatherer;
using UnitsNet.Units;

var gatherer = new GathererPipeline();

var reports = await gatherer.Invoke(new GathererContext());

foreach (var report in reports)
{
    Console.WriteLine($"{report.Input}");
    Console.WriteLine($" - mean time: {report.Performance.Mean.ToUnit(DurationUnit.Millisecond)}");
    Console.WriteLine($" - throughput: {report.Performance.Throughput.ToUnit(BitRateUnit.GigabitPerSecond)}");
    Console.WriteLine($" - dedup ratio: {report.Deduplication.SavedRatio}");
    Console.WriteLine($" - quality ratio: {report.Deduplication.QualityRatio}");
}