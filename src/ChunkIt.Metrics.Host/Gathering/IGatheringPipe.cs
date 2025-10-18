using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Host.Gathering;

internal interface IGatheringPipe
{
    Task<IReadOnlyList<ChunkingReport>> Invoke(
        GatheringContext context,
        AsyncPipeline<GatheringContext, IReadOnlyList<ChunkingReport>> next
    );
}