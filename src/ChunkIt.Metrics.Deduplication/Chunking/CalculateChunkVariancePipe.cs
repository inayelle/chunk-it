using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Deduplication.Chunking;

internal sealed class CalculateChunkVariancePipe : IChunkingPipe
{
    public async Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    )
    {
        var variance = CalculateVariance(context);

        var report = await next(context);

        report.VarianceRatio = variance;

        return report;
    }

    private static float CalculateVariance(ChunkingContext context)
    {
        var minimumChunkSize = context.Partitioner.MinimumChunkSize;
        var averageChunkSize = context.Partitioner.AverageChunkSize;
        var maximumChunkSize = context.Partitioner.MaximumChunkSize;

        var maxLeftDrift = averageChunkSize - minimumChunkSize;
        var maxRightDrift = maximumChunkSize - averageChunkSize;

        var totalDrift = 0f;

        foreach (var chunk in context.Chunks)
        {
            var drift = MathF.Abs(chunk.Length - averageChunkSize);

            var divisor = chunk.Length <= averageChunkSize
                ? maxLeftDrift
                : maxRightDrift;

            totalDrift += drift / divisor;
        }

        return 1.0f - totalDrift / context.Chunks.Count;
    }
}