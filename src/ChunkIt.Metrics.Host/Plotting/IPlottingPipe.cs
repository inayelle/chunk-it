using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Host.Plotting;

internal interface IPlottingPipe
{
    Task Invoke(PlottingContext context, AsyncPipeline<PlottingContext> next);
}