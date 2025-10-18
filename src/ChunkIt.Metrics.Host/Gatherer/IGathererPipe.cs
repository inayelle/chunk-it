using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Host.Gatherer;

internal interface IGathererPipe
{
    Task<IReadOnlyList<ChunkingReport>> Invoke(
        GathererContext context,
        AsyncPipeline<GathererContext, IReadOnlyList<ChunkingReport>> next
    );
}