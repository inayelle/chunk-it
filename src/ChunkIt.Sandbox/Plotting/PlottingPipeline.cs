using AnyKit.Pipelines;

namespace ChunkIt.Sandbox.Plotting;

internal sealed class PlottingPipeline
{
    private readonly AsyncPipeline<PlottingContext> _pipeline;

    public PlottingPipeline()
    {
        var builder = new AsyncPipelineBuilder<PlottingContext>();

        builder.UsePipe(new GenerateDistributionPlotPipe().Invoke);
        builder.UsePipe(new GenerateDeduplicationPlotPipe().Invoke);

        _pipeline = builder.Build();
    }

    public Task Invoke(PlottingContext context)
    {
        return _pipeline.Invoke(context);
    }
}