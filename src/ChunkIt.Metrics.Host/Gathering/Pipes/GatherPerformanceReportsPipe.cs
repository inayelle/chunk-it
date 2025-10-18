using AnyKit.Pipelines;
using ChunkIt.Metrics.Performance;

namespace ChunkIt.Metrics.Host.Gathering.Pipes;

internal sealed class GatherPerformanceReportsPipe : IGatheringPipe
{
    private readonly PerformanceBenchmarkRunner _runner = new();

    public Task<IReadOnlyList<ChunkingReport>> Invoke(
        GatheringContext context,
        AsyncPipeline<GatheringContext, IReadOnlyList<ChunkingReport>> next
    )
    {
        foreach (var (input, report) in _runner.Run())
        {
            context.AddReport(input, report);
        }

        return next(context);
    }
}