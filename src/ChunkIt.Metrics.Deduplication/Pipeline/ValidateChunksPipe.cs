using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Deduplication.Pipeline;

internal sealed class ValidateChunksPipe : IDeduplicationPipe
{
    public Task<DeduplicationReport> Invoke(
        DeduplicationContext context,
        AsyncPipeline<DeduplicationContext, DeduplicationReport> next
    )
    {
        var expectedFileSize = context.Input.SourceFile.Size;
        var actualFileSize = context.Chunks.Sum(chunk => (long)chunk.Length);

        if (expectedFileSize != actualFileSize)
        {
            throw new ApplicationException("Total chunks size doesn't match the original source file size.");
        }

        return next(context);
    }
}