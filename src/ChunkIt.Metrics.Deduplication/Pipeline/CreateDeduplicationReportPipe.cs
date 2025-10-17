using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Deduplication.Pipeline;

internal sealed class CreateDeduplicationReportPipe : IDeduplicationPipe
{
    public Task<DeduplicationReport> Invoke(
        DeduplicationContext context,
        AsyncPipeline<DeduplicationContext, DeduplicationReport> next
    )
    {
        var report = new DeduplicationReport(context.Chunks);

        return Task.FromResult(report);
    }
}