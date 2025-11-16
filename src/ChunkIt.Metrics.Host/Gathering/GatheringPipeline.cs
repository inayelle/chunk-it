using AnyKit.Pipelines;
using ChunkIt.Metrics.Host.Gathering.Pipes;

namespace ChunkIt.Metrics.Host.Gathering;

internal sealed class GatheringPipeline
{
    private readonly AsyncPipeline<GatheringContext, IReadOnlyList<ChunkingReport>> _pipeline;

    public GatheringPipeline(
        bool mockPerformanceReports = false,
        bool mockDeduplicationReports = false
    )
    {
        var builder = new GatheringPipelineBuilder();

        builder.UsePipe<ValidateSourceFilesPipe>();

        if (mockPerformanceReports)
        {
            builder.UsePipe<MockPerformanceReportsPipe>();
        }
        else
        {
            builder.UsePipe<GatherPerformanceReportsPipe>();
        }

        if (mockDeduplicationReports)
        {
            builder.UsePipe<MockDeduplicationReportsPipe>();
        }
        else
        {
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