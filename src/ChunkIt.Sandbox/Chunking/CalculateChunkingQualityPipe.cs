using AnyKit.Pipelines;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class CalculateChunkingQualityPipe : IChunkingPipe
{
    public async Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    )
    {
        var report = await next(context);

        var driftFactor = CalculateDriftFactor(
            expectedAverageChunkSize: context.Partitioner.AverageChunkSize,
            actualAverageChunkSize: report.AverageChunkSize
        );

        var deduplicationFactor = report.SavedRatio;

        var qualityRatio = MathF.Sqrt(driftFactor * deduplicationFactor);

        report.QualityRatio = qualityRatio;

        return report;
    }

    private static float CalculateDriftFactor(int expectedAverageChunkSize, int actualAverageChunkSize)
    {
        var drift = MathF.Abs(expectedAverageChunkSize - actualAverageChunkSize);

        return 1 - drift / expectedAverageChunkSize;
    }
}