using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Deduplication.Plotting;

internal interface IPlottingPipe
{
    Task Invoke(
        PlottingContext context,
        AsyncPipeline<PlottingContext> next
    );
}