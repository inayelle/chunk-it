using AnyKit.Pipelines;

namespace ChunkIt.Metrics.Deduplication.Pipeline;

public sealed class DeduplicationPipeline
{
    private readonly AsyncPipeline<DeduplicationContext, DeduplicationReport> _pipeline;

    public DeduplicationPipeline()
    {
        var builder = new DeduplicationPipelineBuilder();

        builder
            .UsePipe<ChunkSourceFilePipe>()
            .UsePipe<ValidateChunksPipe>()
            .UsePipe<CalculateDeduplicationQualityPipe>()
            .UsePipe<CalculateChunkVariancePipe>()
            .UsePipe<CalculateFileSizePipe>()
            .UsePipe<CalculateAverageChunkSizePipe>()
            .UsePipe<CreateDeduplicationReportPipe>();

        _pipeline = builder.Build();
    }

    public Task<DeduplicationReport> Invoke(DeduplicationContext context)
    {
        return _pipeline.Invoke(context);
    }
}

file sealed class DeduplicationPipelineBuilder : AsyncPipelineBuilder<DeduplicationContext, DeduplicationReport>
{
    public DeduplicationPipelineBuilder UsePipe<TPipe>()
        where TPipe : IDeduplicationPipe, new()
    {
        var pipe = new TPipe();

        base.UsePipe(pipe.Invoke);

        return this;
    }
}