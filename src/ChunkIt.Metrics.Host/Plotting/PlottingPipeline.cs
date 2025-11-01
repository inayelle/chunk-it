using AnyKit.Pipelines;
using ChunkIt.Metrics.Host.Plotting.Pipes;

namespace ChunkIt.Metrics.Host.Plotting;

internal sealed class PlottingPipeline
{
    private readonly AsyncPipeline<PlottingContext> _pipeline;

    public PlottingPipeline()
    {
        var builder = new PlottingPipelineBuilder();

        builder
            .UsePipe<PlotChunkDistributionPipe>();

        _pipeline = builder.Build();
    }

    public Task Invoke(PlottingContext context)
    {
        return _pipeline.Invoke(context);
    }
}

file sealed class PlottingPipelineBuilder : AsyncPipelineBuilder<PlottingContext>
{
    public PlottingPipelineBuilder UsePipe<TPipe>()
        where TPipe : class, IPlottingPipe, new()
    {
        var pipe = new TPipe();

        base.UsePipe(pipe.Invoke);

        return this;
    }
}