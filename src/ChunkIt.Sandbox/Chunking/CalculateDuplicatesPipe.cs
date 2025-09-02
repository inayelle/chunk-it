using AnyKit.Pipelines;

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
                chunk => (chunk.HashString, chunk.Length),
                static (key, chunks) => new
                {
                    Hash = key.HashString,
                    Length = key.Length,
                    Count = chunks.Count(),
                }
            )
            .OrderByDescending(group => group.Count)
            .ToArray();

        var report = await next(context);

        report.DistinctChunkHashes = chunks.Count(chunk => chunk.Count == 1);

        report.DuplicateChunkHashes = chunks.Count(chunk => chunk.Count > 1);

        report.DuplicateChunkEntries = chunks
            .Where(chunk => chunk.Count > 1)
            .Sum(chunk => chunk.Count);

        return report;
    }
}