using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Deduplication.Chunking;

internal sealed class ValidateChunksPipe : IChunkingPipe
{
    public Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    )
    {
        var expectedFileSize = context.SourceFile.Size;
        var actualFileSize = context.Chunks.Sum(chunk => (long)chunk.Length);

        if (expectedFileSize != actualFileSize)
        {
            throw new ApplicationException("Total chunks size doesn't match the original source file size.");
        }

        return next(context);
    }
}