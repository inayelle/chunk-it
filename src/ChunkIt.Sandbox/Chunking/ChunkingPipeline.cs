using System.Runtime.CompilerServices;
using AnyKit.Pipelines;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class ChunkingPipeline
{
    private readonly AsyncPipeline<ChunkingContext, ChunkingReport> _pipeline;

    public ChunkingPipeline()
    {
        var builder = new AsyncPipelineBuilder<ChunkingContext, ChunkingReport>();

        builder.UsePipe(new ExecuteChunkingPipe().Invoke);
        builder.UsePipe(new ValidateChunksPipe().Invoke);
        builder.UsePipe(new CalculateFileSizePipe().Invoke);
        builder.UsePipe(new CalculateIndexSizePipe().Invoke);
        builder.UsePipe(new CalculateDuplicatesPipe().Invoke);

        if (RuntimeFeature.IsDynamicCodeSupported)
        {
            builder.UsePipe(new WriteChunksPipe().Invoke);
        }

        builder.UsePipe(new CreateChunkingReportPipe().Invoke);

        _pipeline = builder.Build();
    }

    public Task<ChunkingReport> Invoke(ChunkingContext context)
    {
        return _pipeline.Invoke(context);
    }
}