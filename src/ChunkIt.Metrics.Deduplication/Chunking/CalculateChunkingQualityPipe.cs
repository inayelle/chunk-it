using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Deduplication.Chunking;

internal sealed class CalculateChunkingQualityPipe : IChunkingPipe
{
    public async Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    )
    {
        var report = await next(context);

        report.QualityRatio = MathF.Sqrt(
            report.VarianceRatio * report.SavedRatio
        );

        return report;
    }
}