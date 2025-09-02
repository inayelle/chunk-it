using AnyKit.Pipelines;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class ValidateChunksPipe : IChunkingPipe
{
    public async Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    )
    {
        var fileInfo = new FileInfo(context.SourceFilePath);

        var expectedFileSize = fileInfo.Length;
        var actualFileSize = context.Chunks.Sum(chunk => (long)chunk.Length);

        var isValid = expectedFileSize == actualFileSize;

        var report = await next(context);
        report.IsValid = isValid;

        return report;
    }
}