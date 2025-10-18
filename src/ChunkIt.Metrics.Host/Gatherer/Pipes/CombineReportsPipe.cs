using AnyKit.Pipelines;
using ChunkIt.Metrics.Inputs;

namespace ChunkIt.Metrics.Host.Gatherer.Pipes;

internal sealed class CombineReportsPipe : IGathererPipe
{
    public Task<IReadOnlyList<ChunkingReport>> Invoke(
        GathererContext context,
        AsyncPipeline<GathererContext, IReadOnlyList<ChunkingReport>> next
    )
    {
        IReadOnlyList<ChunkingReport> reports = InputsProvider
            .Enumerate()
            .Select(context.GatherReport)
            .ToArray();

        return Task.FromResult(reports);
    }
}