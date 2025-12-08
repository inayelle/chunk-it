using AnyKit.Pipelines;
using ChunkIt.Metrics.Deduplication;
using ChunkIt.Metrics.Inputs;

namespace ChunkIt.Metrics.Host.Gathering.Pipes;

internal sealed class MockDeduplicationReportsPipe : IGatheringPipe
{
    private readonly Random _random = new Random(42);

    public async Task<IReadOnlyList<ChunkingReport>> Invoke(
        GatheringContext context,
        AsyncPipeline<GatheringContext, IReadOnlyList<ChunkingReport>> next
    )
    {
        foreach (var input in InputsProvider.Enumerate())
        {
            var report = new DeduplicationReport([])
            {
                AverageChunkSize = _random.Next(input.Partitioner.MinimumChunkSize, input.Partitioner.MaximumChunkSize),
                SavedBytes = 1488,
                SavedRatio = _random.NextSingle(),
                QualityRatio = _random.NextSingle(),
                VarianceRatio = _random.NextSingle(),
            };

            context.AddReport(input, report);
        }

        return await next(context);
    }
}