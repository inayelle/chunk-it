using AnyKit.Pipelines;
using ChunkIt.Common.Extensions;

namespace ChunkIt.Metrics.Deduplication.Pipeline;

internal sealed class CalculateFileSizePipe : IDeduplicationPipe
{
    public async Task<DeduplicationReport> Invoke(
        DeduplicationContext context,
        AsyncPipeline<DeduplicationContext, DeduplicationReport> next
    )
    {
        var report = await next(context);

        var originalFileSize = context.Input.SourceFile.Size;

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