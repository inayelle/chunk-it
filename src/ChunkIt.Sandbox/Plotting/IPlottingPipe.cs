using AnyKit.Pipelines;

namespace ChunkIt.Sandbox.Plotting;

internal interface IPlottingPipe
{
    Task Invoke(
        PlottingContext context,
        AsyncPipeline<PlottingContext> next
    );
}