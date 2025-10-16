using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using ChunkIt.Benchmarks.Extensions;
using ChunkIt.Common.Abstractions;
using Perfolizer.Horology;

namespace ChunkIt.Benchmarks;

internal sealed class ChunkingBenchmarkResult : IEquatable<ChunkingBenchmarkResult>
{
    public required SourceFile SourceFile { get; init; }
    public required IPartitioner Partitioner { get; init; }
    public required Statistics Statistics { get; init; }
    public required BenchmarkCase Benchmark { get; init; }

    public double Throughput => SourceFile.Size / Statistics.Mean;
    public TimeInterval Mean => TimeInterval.FromNanoseconds(Statistics.Mean);

    public string ThroughputText()
    {
        var throughput = Throughput * 1e9 * 8;

        return SizeUnits.BitsPerSecond(throughput);
    }

    public static ChunkingBenchmarkResult FromBenchmark(BenchmarkCase benchmark, Summary summary)
    {
        return new ChunkingBenchmarkResult
        {
            SourceFile = benchmark.GetSourceFile(),
            Partitioner = benchmark.GetPartitioner(),
            Statistics = summary[benchmark]!.ResultStatistics!,
            Benchmark = benchmark,
        };
    }

    public sealed class SourceFileGroup
    {
        public required SourceFile SourceFile { get; init; }
        public required ChunkingBenchmarkResult[] Results { get; init; }
    }

    public bool Equals(ChunkingBenchmarkResult other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return SourceFile.Equals(other.SourceFile) &&
               Partitioner.Equals(other.Partitioner) &&
               Benchmark.Equals(other.Benchmark);
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is ChunkingBenchmarkResult other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SourceFile, Partitioner, Benchmark);
    }
}

file static class SizeUnits
{
    private static readonly string[] Units =
    [
        "b",
        "Kb",
        "Mb",
        "Gb",
        "Tb",
        "Eb",
    ];

    public static string BitsPerSecond(double value)
    {
        var index = 0;
        while (value > 1024.0d)
        {
            value /= 1024.0d;
            index += 1;
        }

        var unit = Units[index];

        value = Math.Round(value, 2);

        return $"{value} {unit}/s";
    }
}