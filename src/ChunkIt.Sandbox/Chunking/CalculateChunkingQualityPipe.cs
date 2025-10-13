using AnyKit.Pipelines;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class CalculateChunkingQualityPipe : IChunkingPipe
{
    private const float Epsilon = 0.25f;

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
        var expectedAverageChunkSize = report.Partitioner.AverageChunkSize;

        var tolerance = (int)Math.Round(expectedAverageChunkSize * Epsilon);

        var toleratedChunks = report
            .Chunks
            .Count(chunk => chunk.Drift(expectedAverageChunkSize) <= tolerance);

        return toleratedChunks / (float)report.Chunks.Count;
    }
}

file static class Extensions
{
    public static int Drift(this ref readonly Chunk chunk, int expectedAverageChunkSize)
    {
        return Math.Abs(chunk.Length - expectedAverageChunkSize);
    }
}