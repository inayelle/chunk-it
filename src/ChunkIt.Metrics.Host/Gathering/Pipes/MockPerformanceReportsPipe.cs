using AnyKit.Pipelines;
using ChunkIt.Metrics.Inputs;
using ChunkIt.Metrics.Performance;

namespace ChunkIt.Metrics.Host.Gathering.Pipes;

internal sealed class MockPerformanceReportsPipe : IGatheringPipe
{
    private static readonly IReadOnlyList<TimeSpan> Values =
    [
        // linux
        TimeSpan.FromMilliseconds(39_545.0), // rabin
        TimeSpan.FromMilliseconds(13_225.7), // gear
        TimeSpan.FromMilliseconds(9_431.4), // fast
        TimeSpan.FromMilliseconds(10_983.9), // ram
        TimeSpan.FromMilliseconds(18_189.6), // ae
        TimeSpan.FromMilliseconds(19_792.6), // seq
        TimeSpan.FromMilliseconds(12_245.0), // twin-mono
        TimeSpan.FromMilliseconds(9_245.9), // twin-duo

        // dotnet
        TimeSpan.FromMilliseconds(41_283.7), // rabin
        TimeSpan.FromMilliseconds(15_922.1), // gear
        TimeSpan.FromMilliseconds(13_752.6), // fast
        TimeSpan.FromMilliseconds(13_845.2), // ram
        TimeSpan.FromMilliseconds(18_204.1), // ae
        TimeSpan.FromMilliseconds(33_274.6), // seq
        TimeSpan.FromMilliseconds(13_999.6), // twin-mono
        TimeSpan.FromMilliseconds(13_944.9), // twin-duo

        // gcc
        TimeSpan.FromMilliseconds(45_538.4), // rabin
        TimeSpan.FromMilliseconds(24_604.4), // gear
        TimeSpan.FromMilliseconds(17_231.5), // fast
        TimeSpan.FromMilliseconds(21_075.8), // ram
        TimeSpan.FromMilliseconds(23_272.0), // ae
        TimeSpan.FromMilliseconds(31_946.4), // seq
        TimeSpan.FromMilliseconds(14_138.3), // twin-mono
        TimeSpan.FromMilliseconds(12_428.5), // twin-duo

        // random
        TimeSpan.FromMilliseconds(13_082.3), // rabin
        TimeSpan.FromMilliseconds(6_350.4), // gear
        TimeSpan.FromMilliseconds(3_024.0), // fast
        TimeSpan.FromMilliseconds(3_201.7), // ram
        TimeSpan.FromMilliseconds(7_018.0), // ae
        TimeSpan.FromMilliseconds(5_314.2), // seq
        TimeSpan.FromMilliseconds(1_213.5), // twin-mono
        TimeSpan.FromMilliseconds(1_474.1), // twin-duo
    ];

    public Task<IReadOnlyList<ChunkingReport>> Invoke(
        GatheringContext context,
        AsyncPipeline<GatheringContext, IReadOnlyList<ChunkingReport>> next
    )
    {
        var inputs = InputsProvider.Enumerate().ToArray();

        if (inputs.Length != Values.Count)
        {
            throw new InvalidOperationException("Invalid number of performance mock values.");
        }

        foreach (var input in inputs)
        {
            var mean = Values[input.Index];

            var report = new PerformanceReport(input.SourceFile, mean.TotalNanoseconds);
            context.AddReport(input, report);
        }

        return next(context);
    }
}