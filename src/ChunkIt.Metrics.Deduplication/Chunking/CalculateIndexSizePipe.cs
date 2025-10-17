using AnyKit.Pipelines;
using ChunkIt.Common.Extensions;

namespace ChunkIt.Metrics.Deduplication.Chunking;

internal sealed class CalculateIndexSizePipe : IChunkingPipe
{
    private const long FileIdSize = 16; // sizeof(Guid)
    private const long OffsetSize = sizeof(long);
    private const long LengthSize = sizeof(int);

    public async Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
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

        var indexRatio = indexBytes / (float)context.SourceFile.Size;

        report.IndexBytes = indexBytes;
        report.IndexRatio = indexRatio;

        return report;
    }
}