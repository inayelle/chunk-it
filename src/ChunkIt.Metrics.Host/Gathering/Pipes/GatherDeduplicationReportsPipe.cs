using AnyKit.Pipelines;
using ChunkIt.Metrics.Deduplication;

namespace ChunkIt.Metrics.Host.Gathering.Pipes;

internal sealed class GatherDeduplicationReportsPipe : IGatheringPipe
{
    private readonly DeduplicationBenchmarkRunner _runner = new();

    public async Task<IReadOnlyList<ChunkingReport>> Invoke(
        GatheringContext context,
        AsyncPipeline<GatheringContext, IReadOnlyList<ChunkingReport>> next
    )
    {
        await foreach (var (input, report) in _runner.Run())
        {
            context.AddReport(input, report);
        }

        return await next(context);
    }
}