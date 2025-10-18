using AnyKit.Pipelines;
using ChunkIt.Metrics.Inputs;

namespace ChunkIt.Metrics.Host.Gathering.Pipes;

internal sealed class CombineReportsPipe : IGatheringPipe
{
    public Task<IReadOnlyList<ChunkingReport>> Invoke(
        GatheringContext context,
        AsyncPipeline<GatheringContext, IReadOnlyList<ChunkingReport>> next
    )
    {
        IReadOnlyList<ChunkingReport> reports = InputsProvider
            .Enumerate()
            .Select(context.GatherReport)
            .ToArray();

        return Task.FromResult(reports);
    }
}