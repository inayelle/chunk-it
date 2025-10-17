using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Deduplication.Chunking;

internal interface IChunkingPipe
{
    Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    );
}