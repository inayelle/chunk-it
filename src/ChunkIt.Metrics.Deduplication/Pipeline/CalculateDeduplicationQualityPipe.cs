using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Deduplication.Pipeline;

internal sealed class CalculateDeduplicationQualityPipe : IDeduplicationPipe
{
    public async Task<DeduplicationReport> Invoke(
        DeduplicationContext context,
        AsyncPipeline<DeduplicationContext, DeduplicationReport> next
    )
    {
        var report = await next(context);

        report.QualityRatio = MathF.Sqrt(
            report.VarianceRatio * report.SavedRatio
        );

        return report;
    }
}