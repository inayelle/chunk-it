using System.Runtime.CompilerServices;
using AnyKit.Pipelines;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class ChunkingPipeline
{
    private readonly AsyncPipeline<ChunkingContext, ChunkingReport> _pipeline;

    public ChunkingPipeline()
    {
        var builder = new ChunkingPipelineBuilder();

        builder
            .UsePipe<ExecuteChunkingPipe>()
            .UsePipe<ValidateChunksPipe>()
            .UsePipe<CalculateChunkingQualityPipe>()
            .UsePipe<CalculateChunkVariancePipe>()
            .UsePipe<CalculateIndexSizePipe>()
            .UsePipe<CalculateFileSizePipe>()
            .UsePipe<CalculateAverageChunkSizePipe>();

        if (RuntimeFeature.IsDynamicCodeSupported)
        {
            builder.UsePipe<WriteChunksPipe>();
        }

        builder.UsePipe<CreateChunkingReportPipe>();

        _pipeline = builder.Build();
    }

    public Task<ChunkingReport> Invoke(ChunkingContext context)
    {
        return _pipeline.Invoke(context);
    }
}

file sealed class ChunkingPipelineBuilder : AsyncPipelineBuilder<ChunkingContext, ChunkingReport>
{
    public ChunkingPipelineBuilder UsePipe<TPipe>()
        where TPipe : IChunkingPipe, new()
    {
        var pipe = new TPipe();

        base.UsePipe(pipe.Invoke);

        return this;
    }
}