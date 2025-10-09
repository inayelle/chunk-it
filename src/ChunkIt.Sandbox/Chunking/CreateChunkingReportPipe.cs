using AnyKit.Pipelines;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class CreateChunkingReportPipe : IChunkingPipe
{
    public Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    )
    {
        var report = new ChunkingReport
        {
            SourceFile = context.SourceFile,
            Partitioner = context.Partitioner,
            Chunks = context.Chunks,
        };

        return Task.FromResult(report);
    }
}