using AnyKit.Pipelines;
using ChunkIt.Common;
using ChunkIt.Common.Extensions;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class CalculateIndexSizePipe : IChunkingPipe
{
    private const long FileIdSize = sizeof(long);
    private const long ChunkIdSize = sizeof(long);
    private const long OffsetSize = sizeof(long);
    private const long LengthSize = sizeof(int);
    private const long HashSize = 32;

    public async Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    )
    {
        var report = await next(context);

        var uniqueChunksCount = context
            .Chunks
            .DistinctBy(chunk => chunk.Hash, ByteArrayEqualityComparer.Instance)
            .LongCount();

        var indexBytes = (FileIdSize + ChunkIdSize + OffsetSize + LengthSize + HashSize) * uniqueChunksCount;
        var indexRatio = indexBytes / (float)context.SourceFile.Size * 100;

        report.IndexBytes = indexBytes;
        report.IndexRatio = indexRatio;

        return report;
    }
}