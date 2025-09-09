using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Reports;
using ChunkIt.Common;
using ChunkIt.Common.Abstractions;
using ChunkIt.Hashers;
using ChunkIt.Partitioners.AsymmetricExtremum;
using ChunkIt.Partitioners.Fixed;
using ChunkIt.Partitioners.Gear;
using ChunkIt.Partitioners.RapidAsymmetricMaximum;

namespace ChunkIt.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(ChunkingBenchmarkConfig))]
public class ChunkingBenchmark
{
    private const int Kilobyte = 1024;
    private const int BufferSize = Kilobyte * 1000 * 4;

    private FileStream _sourceFileStream;

    [ParamsSource(nameof(GenerateSourceFilePaths))]
    public SourceFilePath SourceFilePath { get; set; }

    [ParamsSource(nameof(GeneratePartitioners))]
    public IPartitioner Partitioner { get; set; }

    [Benchmark]
    public async Task<ulong> Run()
    {
        var reader = new ChunkReader(Partitioner, NoneHasher.Instance, BufferSize);

        ulong length = 0;

        await foreach (var chunkLength in reader.ReadChunkLengthsAsync(_sourceFileStream).ConfigureAwait(false))
        {
            length += (ulong)chunkLength;
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
            FileShare.None,
            bufferSize: BufferSize,
            options: FileOptions.Asynchronous | FileOptions.SequentialScan
        );
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _sourceFileStream.Seek(0, SeekOrigin.Begin);
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _sourceFileStream.Dispose();
    }

    public static IEnumerable<SourceFilePath> GenerateSourceFilePaths()
    {
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/linux-6.16.4.tar";
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/linux-combined.tar";

        yield return "/home/ina/downloads/gcc/gcc.tar";
    }

    public static IEnumerable<IPartitioner> GeneratePartitioners()
    {
        // int[] minimumChunkSizes = [1 * Kilobyte, 2 * Kilobyte, 4 * Kilobyte];
        int[] minimumChunkSizes = [4 * Kilobyte];

        foreach (var minimumChunkSize in minimumChunkSizes)
        {
            var maximumChunkSize = minimumChunkSize * 8;
            var averageChunkSize = minimumChunkSize + (maximumChunkSize - minimumChunkSize) / 2;

            yield return new FixedPartitioner(
                chunkSize: averageChunkSize
            );

            yield return new GearPartitioner(
                minimumChunkSize: minimumChunkSize,
                averageChunkSize: averageChunkSize,
                maximumChunkSize: maximumChunkSize,
                gearTable: GearTable.Predefined(rotations: 0)
            );

            yield return new FastPartitioner(
                minimumChunkSize: minimumChunkSize,
                averageChunkSize: averageChunkSize,
                maximumChunkSize: maximumChunkSize,
                normalizationLevel: 3,
                gearTable: GearTable.Predefined(rotations: 0)
            );

            yield return new TwinPartitioner(
                minimumChunkSize: minimumChunkSize,
                averageChunkSize: averageChunkSize,
                maximumChunkSize: maximumChunkSize,
                normalizationLevel: 3,
                leftGearTable: GearTable.Predefined(rotations: 0),
                rightGearTable: GearTable.Predefined(rotations: 17)
            );

            yield return new RapidAsymmetricMaximumPartitioner(
                minimumChunkSize: minimumChunkSize,
                averageChunkSize: averageChunkSize,
                maximumChunkSize: maximumChunkSize
            );

            yield return new AsymmetricExtremumPartitioner(
                minimumChunkSize: minimumChunkSize,
                averageChunkSize: averageChunkSize,
                maximumChunkSize: maximumChunkSize
            );
        }
    }
}

public sealed class ChunkingBenchmarkConfig : ManualConfig
{
    public ChunkingBenchmarkConfig()
    {
        SummaryStyle = SummaryStyle.Default.WithMaxParameterColumnWidth(64);

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