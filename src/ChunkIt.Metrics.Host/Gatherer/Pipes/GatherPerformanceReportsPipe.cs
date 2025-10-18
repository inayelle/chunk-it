using AnyKit.Pipelines;
using ChunkIt.Metrics.Performance;

namespace ChunkIt.Metrics.Host.Gatherer.Pipes;

internal sealed class GatherPerformanceReportsPipe : IGathererPipe
{
    private readonly PerformanceBenchmarkRunner _runner = new();

    public Task<IReadOnlyList<ChunkingReport>> Invoke(
        GathererContext context,
        AsyncPipeline<GathererContext, IReadOnlyList<ChunkingReport>> next
    )
    {
        foreach (var (input, report) in _runner.Run())
        {
            context.AddReport(input, report);
        }

        return next(context);
    }
}