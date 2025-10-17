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

        var varianceRatio = report.VarianceRatio;
        var deduplicationRatio = report.SavedRatio;

        var qualityRatio = MathF.Sqrt(varianceRatio * deduplicationRatio);
        report.QualityRatio = qualityRatio;

        return report;
    }
}