using AnyKit.Pipelines;
using ChunkIt.Common.Extensions;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class CalculateFileSizePipe : IChunkingPipe
{
    public async Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    )
    {
        var report = await next(context);

        var originalFileSize = context.SourceFile.Size;

        var compressedFileSize = context
            .Chunks
            .DistinctByHash()
            .Sum(chunk => (long)chunk.Length);

        var savedBytes = originalFileSize - compressedFileSize;
        var savedRatio = savedBytes / (float)originalFileSize;

        report.SavedBytes = savedBytes;
        report.SavedRatio = savedRatio;

        return report;
    }
}