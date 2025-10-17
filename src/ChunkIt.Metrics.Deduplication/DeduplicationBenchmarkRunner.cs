using ChunkIt.Common.Abstractions;
using ChunkIt.Metrics.Deduplication.Pipeline;
using ChunkIt.Metrics.Inputs;

namespace ChunkIt.Metrics.Deduplication;

public class DeduplicationBenchmarkRunner : IBenchmarkRunner<DeduplicationReport>
{
    private readonly DeduplicationPipeline _pipeline = new();

    public async Task<Dictionary<Input, DeduplicationReport>> Run()
    {
        return await RunCore().ToDictionaryAsync(entry => entry.Input, entry => entry.Report);
    }

    private async IAsyncEnumerable<(Input Input, DeduplicationReport Report)> RunCore()
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