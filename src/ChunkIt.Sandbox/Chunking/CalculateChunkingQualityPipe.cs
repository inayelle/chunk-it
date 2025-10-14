using AnyKit.Pipelines;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class CalculateChunkingQualityPipe : IChunkingPipe
{
    public async Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    )
    {
        var report = await next(context);

        var driftFactor = CalculateDriftFactor(report);
        var deduplicationFactor = report.SavedRatio;

        var qualityRatio = MathF.Sqrt(driftFactor * deduplicationFactor);
        report.QualityRatio = qualityRatio;

        return report;
    }

    private static float CalculateDriftFactor(ChunkingReport report)
    {
        var minimumChunkSize = report.Partitioner.MinimumChunkSize;
        var averageChunkSize = report.Partitioner.AverageChunkSize;
        var maximumChunkSize = report.Partitioner.MaximumChunkSize;

        var maxLeftDrift = averageChunkSize - minimumChunkSize;
        var maxRightDrift = maximumChunkSize - averageChunkSize;

        var totalDrift = 0f;

        foreach (var chunk in report.Chunks)
        {
            var drift = MathF.Abs(chunk.Length - averageChunkSize);

            var divisor = chunk.Length <= averageChunkSize
                ? maxLeftDrift
                : maxRightDrift;

            totalDrift += drift / divisor;
        }

        return 1.0f - totalDrift / report.Chunks.Count;
    }
}