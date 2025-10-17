using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Deduplication.Pipeline;

internal sealed class CalculateChunkVariancePipe : IDeduplicationPipe
{
    public async Task<DeduplicationReport> Invoke(
        DeduplicationContext context,
        AsyncPipeline<DeduplicationContext, DeduplicationReport> next
    )
    {
        var variance = CalculateVariance(context);

        var report = await next(context);

        report.VarianceRatio = variance;

        return report;
    }

    private static float CalculateVariance(DeduplicationContext context)
    {
        var minimumChunkSize = context.Input.Partitioner.MinimumChunkSize;
        var averageChunkSize = context.Input.Partitioner.AverageChunkSize;
        var maximumChunkSize = context.Input.Partitioner.MaximumChunkSize;

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