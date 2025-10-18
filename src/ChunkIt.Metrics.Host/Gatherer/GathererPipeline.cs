using AnyKit.Pipelines;
using ChunkIt.Metrics.Host.Gatherer.Pipes;

namespace ChunkIt.Metrics.Host.Gatherer;

internal sealed class GathererPipeline
{
    private readonly AsyncPipeline<GathererContext, IReadOnlyList<ChunkingReport>> _pipeline;

    public GathererPipeline()
    {
        var builder = new GathererPipelineBuilder();

        builder
            .UsePipe<GatherPerformanceReportsPipe>()
            .UsePipe<GatherDeduplicationReportsPipe>()
            .UsePipe<CombineReportsPipe>();

        _pipeline = builder.Build();
    }

    public Task<IReadOnlyList<ChunkingReport>> Invoke(GathererContext context)
    {
        return _pipeline.Invoke(context);
    }
}

file sealed class GathererPipelineBuilder
    : AsyncPipelineBuilder<GathererContext, IReadOnlyList<ChunkingReport>>
{
    public GathererPipelineBuilder UsePipe<TPipe>()
        where TPipe : class, IGathererPipe, new()
    {
        var pipe = new TPipe();

        base.UsePipe(pipe.Invoke);

        return this;
    }
}