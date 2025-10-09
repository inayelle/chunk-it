using AnyKit.Pipelines;
using ChunkIt.Common.Extensions;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class CalculateDuplicatesPipe : IChunkingPipe
{
    public async Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    )
    {
        var chunks = context.Chunks
            .GroupBy(
                chunk => chunk.Hash,
                static (hash, chunks) => new
                {
                    Hash = hash,
                    Count = chunks.Count(),
                },
                ByteArrayEqualityComparer.Instance
            )
            .OrderByDescending(group => group.Count)
            .ToArray();

        var report = await next(context);

        report.DistinctChunkHashes = chunks.Count(group => group.Count == 1);

        report.DuplicateChunkHashes = chunks.Count(group => group.Count > 1);

        report.DuplicateChunkEntries = chunks
            .Where(chunk => chunk.Count > 1)
            .Sum(chunk => chunk.Count);

        return report;
    }
}