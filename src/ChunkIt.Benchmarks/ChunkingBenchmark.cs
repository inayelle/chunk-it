using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Reports;
using ChunkIt.Abstractions;
using ChunkIt.Hashers;
using ChunkIt.Partitioners.Gear;
using ChunkIt.Partitioners.Ram;
using ChunkIt.Partitioners.Sequential;

namespace ChunkIt.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(ChunkingBenchmarkConfig))]
public class ChunkingBenchmark
{
    private const string SourceDirectoryPath = "/storage/ina/workspace/personal/ChunkIt/inputs";

    private const int Kilobyte = 1024;

    private const int MinimumChunkSize = 1 * Kilobyte;
    private const int AverageChunkSize = 4 * Kilobyte;
    private const int MaximumChunkSize = 8 * Kilobyte;

    private Stream _sourceFileStream;

    [ParamsSource(nameof(GenerateSourceFilePaths))]
    public string SourceFilePath { get; set; }

    [ParamsSource(nameof(GenerateChunkReaders))]
    public ChunkReader ChunkReader { get; set; }

    [Benchmark]
    public async Task<ulong> Run()
    {
        _sourceFileStream.Seek(0, SeekOrigin.Begin);

        ulong length = 0;

        await foreach (var chunk in ChunkReader.ReadAsync(_sourceFileStream).ConfigureAwait(false))
        {
            length += (ulong)chunk.Length;
        }

        return length;
    }

    [GlobalSetup]
    public void GlobalSetup()
    {
        using var fileStream = new FileStream(SourceFilePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.None
        );

        _sourceFileStream = new MemoryStream();

        fileStream.CopyTo(_sourceFileStream);
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _sourceFileStream.Dispose();
    }

    public static IEnumerable<object> GenerateSourceFilePaths()
    {
        yield return Path.Combine(SourceDirectoryPath, "1MB-original.json");
        yield return Path.Combine(SourceDirectoryPath, "25MB-original.json");
    }

    public static IEnumerable<object> GenerateChunkReaders()
    {
        yield return new ChunkReader(
            partitioner: new GearPartitioner(
                gearTable: new RandomGearTable(new Random(42)),
                minimumChunkSize: MinimumChunkSize,
                averageChunkSize: AverageChunkSize,
                maximumChunkSize: MaximumChunkSize,
                normalizationLevel: 2
            ),
            hasher: NoneHasher.Instance,
            bufferSize: MaximumChunkSize
        );

        yield return new ChunkReader(
            partitioner: new RamPartitioner(
                minimumChunkSize: MinimumChunkSize,
                maximumChunkSize: MaximumChunkSize,
                windowSize: 64
            ),
            hasher: NoneHasher.Instance,
            bufferSize: MaximumChunkSize
        );

        yield return new ChunkReader(
            partitioner: new SequentialPartitioner(
                mode: SequentialPartitionerMode.Increasing,
                minimumChunkSize: MinimumChunkSize,
                averageChunkSize: AverageChunkSize,
                maximumChunkSize: MaximumChunkSize,
                sequenceLength: 4,
                skipLength: 16,
                skipTrigger: 8
            ),
            hasher: NoneHasher.Instance,
            bufferSize: MaximumChunkSize
        );

        yield return new ChunkReader(
            partitioner: new SequentialPartitioner(
                mode: SequentialPartitionerMode.Decreasing,
                minimumChunkSize: MinimumChunkSize,
                averageChunkSize: AverageChunkSize,
                maximumChunkSize: MaximumChunkSize,
                sequenceLength: 4,
                skipLength: 12,
                skipTrigger: 3
            ),
            hasher: NoneHasher.Instance,
            bufferSize: MaximumChunkSize
        );
    }
}

file sealed class ChunkingBenchmarkConfig : ManualConfig
{
    public ChunkingBenchmarkConfig()
    {
        SummaryStyle = SummaryStyle.Default.WithMaxParameterColumnWidth(64);

        AddDiagnoser(new EventPipeProfiler(EventPipeProfile.CpuSampling));

        WithOptions(ConfigOptions.DisableLogFile);
    }
}