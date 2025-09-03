using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Reports;
using ChunkIt.Common;
using ChunkIt.Common.Abstractions;
using ChunkIt.Hashers;
using ChunkIt.Partitioners.Entropy;
using ChunkIt.Partitioners.Fixed;
using ChunkIt.Partitioners.Gear;
using ChunkIt.Partitioners.MeanShift;
using ChunkIt.Partitioners.Ram;
using ChunkIt.Partitioners.Sequential;

namespace ChunkIt.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(ChunkingBenchmarkConfig))]
public class ChunkingBenchmark
{
    private const int Kilobyte = 1024;

    private FileStream _sourceFileStream;

    [ParamsSource(nameof(GenerateSourceFilePaths))]
    public SourceFilePath SourceFilePath { get; set; }

    [ParamsSource(nameof(GeneratePartitioners))]
    public IPartitioner Partitioner { get; set; }

    [Benchmark]
    public async Task<ulong> Run()
    {
        _sourceFileStream.Seek(0, SeekOrigin.Begin);

        var reader = new ChunkReader(Partitioner, NoneHasher.Instance, 32 * Kilobyte);

        ulong length = 0;

        await foreach (var chunk in reader.ReadAsync(_sourceFileStream).ConfigureAwait(false))
        {
            length += (ulong)chunk.Length;
        }

        return length;
    }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _sourceFileStream = new FileStream(
            SourceFilePath.Path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.None
        );
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _sourceFileStream.Dispose();
    }

    public static IEnumerable<SourceFilePath> GenerateSourceFilePaths()
    {
        yield return "/storage/ina/workspace/personal/ChunkIt/inputs/linux-6.16.4.tar";
        yield return "/storage/ina/workspace/personal/ChunkIt/inputs/linux-combined.tar";
    }

    public static IEnumerable<IPartitioner> GeneratePartitioners()
    {
        int[] minimumChunkSizes = [1 * Kilobyte, 2 * Kilobyte, 4 * Kilobyte];

        foreach (var minimumChunkSize in minimumChunkSizes)
        {
            var maximumChunkSize = minimumChunkSize * 8;
            var averageChunkSize = minimumChunkSize + (maximumChunkSize - minimumChunkSize) / 2;

            yield return new FixedPartitioner(
                chunkSize: averageChunkSize
            );

            yield return new GearPartitioner(
                gearTable: new StaticGearTable(),
                minimumChunkSize: minimumChunkSize,
                averageChunkSize: averageChunkSize,
                maximumChunkSize: maximumChunkSize,
                normalizationLevel: 3
            );

            yield return new CentricGearPartitioner(
                gearTable: new StaticGearTable(),
                minimumChunkSize: minimumChunkSize,
                averageChunkSize: averageChunkSize,
                maximumChunkSize: maximumChunkSize,
                normalizationLevel: 3
            );

            yield return new SlidingGearPartitioner(
                gearTable: new StaticGearTable(),
                minimumChunkSize: minimumChunkSize,
                averageChunkSize: averageChunkSize,
                maximumChunkSize: maximumChunkSize,
                normalizationLevel: 3
            );

            yield return new RamPartitioner(
                minimumChunkSize: minimumChunkSize,
                maximumChunkSize: maximumChunkSize,
                windowSize: averageChunkSize
            );

            yield return new SequentialPartitioner(
                mode: SequentialPartitionerMode.Increasing,
                minimumChunkSize: minimumChunkSize,
                averageChunkSize: averageChunkSize,
                maximumChunkSize: maximumChunkSize,
                sequenceLength: 5,
                skipLength: 50,
                skipTrigger: 256
            );

            yield return new SequentialPartitioner(
                mode: SequentialPartitionerMode.Decreasing,
                minimumChunkSize: minimumChunkSize,
                averageChunkSize: averageChunkSize,
                maximumChunkSize: maximumChunkSize,
                sequenceLength: 5,
                skipLength: 50,
                skipTrigger: 256
            );

            yield return new AdaptiveSequentialPartitioner(
                mode: SequentialPartitionerMode.Increasing,
                minimumChunkSize: minimumChunkSize,
                averageChunkSize: averageChunkSize,
                maximumChunkSize: maximumChunkSize,
                sequenceLength: 5,
                skipLength: 50,
                skipTrigger: 256
            );

            yield return new AdaptiveSequentialPartitioner(
                mode: SequentialPartitionerMode.Decreasing,
                minimumChunkSize: minimumChunkSize,
                averageChunkSize: averageChunkSize,
                maximumChunkSize: maximumChunkSize,
                sequenceLength: 5,
                skipLength: 50,
                skipTrigger: 256
            );

            yield return new MeanShiftPartitioner(
                minimumChunkSize: minimumChunkSize,
                averageChunkSize: averageChunkSize,
                maximumChunkSize: maximumChunkSize
            );

            yield return new EntropyPartitioner(
                minimumChunkSize: minimumChunkSize,
                averageChunkSize: averageChunkSize,
                maximumChunkSize: maximumChunkSize,
                windowSize: 64,
                lowThresholdBits: 1.25,
                highThresholdBits: 1.85
            );
        }
    }
}

public sealed class ChunkingBenchmarkConfig : ManualConfig
{
    public ChunkingBenchmarkConfig()
    {
        SummaryStyle = SummaryStyle.Default.WithMaxParameterColumnWidth(64);

        AddDiagnoser(new EventPipeProfiler(EventPipeProfile.CpuSampling));

        WithOptions(ConfigOptions.DisableLogFile);
    }
}

public sealed class SourceFilePath
{
    public string Path { get; }

    public SourceFilePath(string path)
    {
        Path = path;
    }

    public override string ToString()
    {
        return System.IO.Path.GetFileName(Path);
    }

    public static implicit operator SourceFilePath(string path)
    {
        return new SourceFilePath(path);
    }
}