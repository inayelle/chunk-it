using ChunkIt.Metrics.Host.Gathering;
using UnitsNet.Units;

var gatheringPipeline = new GatheringPipeline();

var reports = await gatheringPipeline.Invoke(new GatheringContext());

foreach (var report in reports)
{
    Console.WriteLine($"{report.Input}");
    Console.WriteLine($" - mean time: {report.Performance.Mean.ToUnit(DurationUnit.Millisecond)}");
    Console.WriteLine($" - throughput: {report.Performance.Throughput.ToUnit(BitRateUnit.GigabitPerSecond)}");
    Console.WriteLine($" - dedup ratio: {report.Deduplication.SavedRatio}");
    Console.WriteLine($" - variance ratio: {report.Deduplication.VarianceRatio}");
    Console.WriteLine($" - quality ratio: {report.Deduplication.QualityRatio}");
}