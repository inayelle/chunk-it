using AnyKit.Pipelines;
using ChunkIt.Common.Extensions;

namespace ChunkIt.Metrics.Deduplication.Pipeline;

internal sealed class CalculateIndexSizePipe : IDeduplicationPipe
{
    private const long FileIdSize = 16; // sizeof(Guid)
    private const long OffsetSize = sizeof(long);
    private const long LengthSize = sizeof(int);

    public async Task<DeduplicationReport> Invoke(
        DeduplicationContext context,
        AsyncPipeline<DeduplicationContext, DeduplicationReport> next
    )
    {
        var report = await next(context);

        var uniqueEntries = context
            .Chunks
            .GroupByHash((hash, chunks) => new
                {
                    HashSize = hash.Length,
                    ChunksCount = chunks.Count(),
                }
            )
            .ToArray();

        var indexBytes = (FileIdSize + LengthSize) * uniqueEntries.Length +
                         uniqueEntries.Sum(entry => entry.HashSize + OffsetSize * entry.ChunksCount);

        var indexRatio = indexBytes / (float)context.Input.SourceFile.Size;

        report.IndexBytes = indexBytes;
        report.IndexRatio = indexRatio;

        return report;
    }
}