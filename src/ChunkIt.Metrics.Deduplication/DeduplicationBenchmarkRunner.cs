using ChunkIt.Common.Abstractions;
using ChunkIt.Metrics.Deduplication.Pipeline;
using ChunkIt.Metrics.Inputs;

namespace ChunkIt.Metrics.Deduplication;

public class DeduplicationBenchmarkRunner
{
    private readonly DeduplicationPipeline _pipeline = new();

    public async IAsyncEnumerable<(Input Input, DeduplicationReport Report)> Run()
    {
        foreach (var input in InputsProvider.Enumerate())
        {
            var report = await GenerateReport(input);

            yield return (input, report);
        }
    }

    private async Task<DeduplicationReport> GenerateReport(Input input)
    {
        var context = new DeduplicationContext(input, _ => { });

        return await _pipeline.Invoke(context);
    }
}