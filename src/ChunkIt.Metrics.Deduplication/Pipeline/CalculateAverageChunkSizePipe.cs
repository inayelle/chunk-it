using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Deduplication.Pipeline;

internal sealed class CalculateAverageChunkSizePipe : IDeduplicationPipe
{
    public async Task<DeduplicationReport> Invoke(
        DeduplicationContext context,
        AsyncPipeline<DeduplicationContext, DeduplicationReport> next
    )
    {
        var report = await next(context);

        var averageChunkSize = context.Chunks.Average(chunk => chunk.Length);
        report.AverageChunkSize = (int)Math.Ceiling(averageChunkSize);

        return report;
    }
}