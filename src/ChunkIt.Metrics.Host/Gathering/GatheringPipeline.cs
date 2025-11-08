using AnyKit.Pipelines;
using ChunkIt.Metrics.Host.Gathering.Pipes;

namespace ChunkIt.Metrics.Host.Gathering;

internal sealed class GatheringPipeline
{
    private readonly AsyncPipeline<GatheringContext, IReadOnlyList<ChunkingReport>> _pipeline;

    public GatheringPipeline(bool mock = false)
    {
        var builder = new GatheringPipelineBuilder();

        if (mock)
        {
            builder.UsePipe<MockPerformanceReportsPipe>();
            builder.UsePipe<MockDeduplicationReportsPipe>();
        }
        else
        {
            builder.UsePipe<ValidateSourceFilesPipe>();
            builder.UsePipe<GatherPerformanceReportsPipe>();
            builder.UsePipe<GatherDeduplicationReportsPipe>();
        }

        builder.UsePipe<CombineReportsPipe>();

        _pipeline = builder.Build();
    }

    public Task<IReadOnlyList<ChunkingReport>> Invoke(GatheringContext context)
    {
        return _pipeline.Invoke(context);
    }
}

file sealed class GatheringPipelineBuilder
    : AsyncPipelineBuilder<GatheringContext, IReadOnlyList<ChunkingReport>>
{
    public GatheringPipelineBuilder UsePipe<TPipe>()
        where TPipe : class, IGatheringPipe, new()
    {
        var pipe = new TPipe();

        base.UsePipe(pipe.Invoke);

        return this;
    }
}