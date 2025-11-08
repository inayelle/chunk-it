using BenchmarkDotNet.Mathematics;
using ChunkIt.Common.Abstractions;
using UnitsNet;

namespace ChunkIt.Metrics.Performance;

public sealed class PerformanceReport
{
    public Duration Mean { get; }
    public BitRate Throughput { get; }

    public PerformanceReport(SourceFile sourceFile, Statistics statistics)
    {
        Mean = Duration.FromNanoseconds(statistics.Mean);

        Throughput = BitRate.FromBytesPerSecond(sourceFile.Size / Mean.Seconds);
    }

    public PerformanceReport(SourceFile sourceFile, double meanNanoseconds)
    {
        Mean = Duration.FromNanoseconds(meanNanoseconds);

        Throughput = BitRate.FromBytesPerSecond(sourceFile.Size / Mean.Seconds);
    }
}