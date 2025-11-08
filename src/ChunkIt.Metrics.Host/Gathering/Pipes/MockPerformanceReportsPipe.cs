using AnyKit.Pipelines;
using ChunkIt.Metrics.Inputs;
using ChunkIt.Metrics.Performance;

namespace ChunkIt.Metrics.Host.Gathering.Pipes;

internal sealed class MockPerformanceReportsPipe : IGatheringPipe
{
    private readonly Random _random = new Random(42);

    public Task<IReadOnlyList<ChunkingReport>> Invoke(
        GatheringContext context,
        AsyncPipeline<GatheringContext, IReadOnlyList<ChunkingReport>> next
    )
    {
        foreach (var input in InputsProvider.Enumerate())
        {
            var milliseconds = _random.Next(300, 1200);
            var nanoseconds = TimeSpan.FromMilliseconds(milliseconds).TotalNanoseconds;

            var report = new PerformanceReport(input.SourceFile, nanoseconds);
            context.AddReport(input, report);
        }

        return next(context);
    }
}