using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using ChunkIt.Benchmarks.Extensions;
using ChunkIt.Common.Abstractions;
using UnitsNet;

namespace ChunkIt.Benchmarks;

internal sealed class ChunkingBenchmarkResult : IEquatable<ChunkingBenchmarkResult>
{
    public SourceFile SourceFile { get; }
    public IPartitioner Partitioner { get; }
    public Statistics Statistics { get; }
    public BenchmarkCase Benchmark { get; }

    public Duration Mean { get; }
    public BitRate Throughput { get; }

    public ChunkingBenchmarkResult(Summary summary, BenchmarkCase benchmark)
    {
        SourceFile = benchmark.GetSourceFile();
        Partitioner = benchmark.GetPartitioner();
        Statistics = summary[benchmark]!.ResultStatistics!;
        Benchmark = benchmark;

        Mean = Duration.FromNanoseconds(Statistics.Mean);
        Throughput = BitRate.FromBytesPerSecond(
            bytespersecond: SourceFile.Size / Mean.Seconds
        );
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