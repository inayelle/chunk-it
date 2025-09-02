using AnyKit.Pipelines;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class CreateSummaryPipe : IChunkingPipe
{
    public Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    )
    {
        var report = new ChunkingReport
        {
            FileName = Path.GetFileName(context.SourceFilePath),
            Partitioner = context.Partitioner,
            Chunks = context.Chunks,
        };

        return Task.FromResult(report);
    }
}