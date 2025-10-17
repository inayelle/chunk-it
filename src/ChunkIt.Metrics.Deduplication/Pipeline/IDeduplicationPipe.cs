using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Deduplication.Pipeline;

internal interface IDeduplicationPipe
{
    Task<DeduplicationReport> Invoke(
        DeduplicationContext context,
        AsyncPipeline<DeduplicationContext, DeduplicationReport> next
    );
}