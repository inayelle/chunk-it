using AnyKit.Pipelines;

namespace ChunkIt.Sandbox.Chunking;

internal interface IChunkingPipe
{
    Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    );
}