using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Reports;
using ChunkIt.Abstractions;
using ChunkIt.Hashers;
using ChunkIt.Partitioners.Gear;

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
        yield return "/home/ina/downloads/linux/linux-6.16.4.tar";
    }

    public static IEnumerable<object> GenerateChunkReaders()
    {
        yield return new ChunkReader(
            partitioner: new GearPartitioner(
                gearTable: new StaticGearTable(),
                minimumChunkSize: MinimumChunkSize,
                averageChunkSize: AverageChunkSize,
                maximumChunkSize: MaximumChunkSize,
                normalizationLevel: 3
            ),
            hasher: NoneHasher.Instance,
            bufferSize: MaximumChunkSize
        );

        yield return new ChunkReader(
            partitioner: new CentricGearPartitioner(
                gearTable: new StaticGearTable(),
                minimumChunkSize: MinimumChunkSize,
                averageChunkSize: AverageChunkSize,
                maximumChunkSize: MaximumChunkSize,
                normalizationLevel: 3
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