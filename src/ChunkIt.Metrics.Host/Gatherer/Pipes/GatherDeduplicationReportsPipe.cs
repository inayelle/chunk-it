using AnyKit.Pipelines;
using ChunkIt.Metrics.Deduplication;

namespace ChunkIt.Metrics.Host.Gatherer.Pipes;

internal sealed class GatherDeduplicationReportsPipe : IGathererPipe
{
    private readonly DeduplicationBenchmarkRunner _runner = new();

    public async Task<IReadOnlyList<ChunkingReport>> Invoke(
        GathererContext context,
        AsyncPipeline<GathererContext, IReadOnlyList<ChunkingReport>> next
    )
    {
        await foreach (var (input, report) in _runner.Run())
        {
            context.AddReport(input, report);
        }

        return await next(context);
    }
}