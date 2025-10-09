using AnyKit.Pipelines;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class CalculateAverageChunkSizePipe : IChunkingPipe
{
    public async Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    )
    {
        var report = await next(context);

        var averageChunkSize = context.Chunks.Average(chunk => chunk.Length);

        report.AverageChunkSize = (int)Math.Ceiling(averageChunkSize);

        return report;
    }
}