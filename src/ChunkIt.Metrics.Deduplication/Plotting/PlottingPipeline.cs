using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Deduplication.Plotting;

internal sealed class PlottingPipeline
{
    private readonly AsyncPipeline<PlottingContext> _pipeline;

    public PlottingPipeline()
    {
        var builder = new PlottingPipelineBuilder();

        builder
            .UsePipe<GenerateDistributionPlotPipe>()
            .UsePipe<GenerateDeduplicationPlotPipe>()
            .UsePipe<GenerateQualityPlotPipe>()
            .UsePipe<GenerateVariancePlotPipe>();

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
        where TPipe : IPlottingPipe, new()
    {
        var pipe = new TPipe();

        base.UsePipe(pipe.Invoke);

        return this;
    }
}