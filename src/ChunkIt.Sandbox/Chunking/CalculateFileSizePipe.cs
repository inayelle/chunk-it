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
        var originalFileSize = context.SourceFile.Size;

        var compressedFileSize = context
            .Chunks
            .DistinctBy(chunk => chunk.Hash, ByteArrayEqualityComparer.Instance)
            .Sum(chunk => (long)chunk.Length);

        var savedBytes = originalFileSize - compressedFileSize;
        var savedRatio = savedBytes / (float)originalFileSize * 100;

        var report = await next(context);

        report.OriginalFileSize = originalFileSize;
        report.CompressedFileSize = compressedFileSize;
        report.SavedBytes = savedBytes;
        report.SavedRatio = savedRatio;

        return report;
    }
}