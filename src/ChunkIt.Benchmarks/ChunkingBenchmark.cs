using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using ChunkIt.Common;
using ChunkIt.Common.Abstractions;
using ChunkIt.Hashers;
using ChunkIt.Partitioners.AsymmetricExtremum;
using ChunkIt.Partitioners.Fixed;
using ChunkIt.Partitioners.Gear;
using ChunkIt.Partitioners.Rabin;
using ChunkIt.Partitioners.RapidAsymmetricMaximum;
using ChunkIt.Partitioners.Sequential;

namespace ChunkIt.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(ChunkingBenchmarkConfig))]
public class ChunkingBenchmark
{
    private const int Kilobyte = 1024;
    private const int BufferSize = Kilobyte * 1000 * 4;

    private const int MinimumChunkSize = 8 * Kilobyte;
    private const int AverageChunkSize = 16 * Kilobyte;
    private const int MaximumChunkSize = 32 * Kilobyte;

    private FileStream _sourceFileStream;

    [ParamsSource(nameof(EnumerateSourceFilePaths))]
    public SourceFile SourceFile { get; set; }

    [ParamsSource(nameof(EnumeratePartitioners))]
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
            SourceFile.Path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
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

    public static IEnumerable<SourceFile> EnumerateSourceFilePaths()
    {
        yield return "/storage/ina/workspace/personal/ChunkIt/inputs/gcc/gcc.tar";
    }

    public static IEnumerable<IPartitioner> EnumeratePartitioners()
    {
        yield return new FixedPartitioner(chunkSize: AverageChunkSize);

        yield return new RabinPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize
        );

        yield return new GearPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            gearTable: GearTable.Predefined(rotations: 0)
        );

        yield return new FastPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3,
            gearTable: GearTable.Predefined(rotations: 0)
        );

        yield return new TwinPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 5,
            gearTable: GearTable.Predefined(rotations: 0)
        );

        yield return new TwinPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 5,
            leftGearTable: GearTable.Predefined(rotations: 0),
            rightGearTable: GearTable.Predefined(rotations: 13)
        );

        yield return new RapidAsymmetricMaximumPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize
        );

        yield return new AsymmetricExtremumPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize
        );

        yield return new SequentialPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            mode: SequentialPartitionerMode.Increasing,
            sequenceLength: 5,
            skipLength: 512,
            skipTrigger: 50
        );

        yield return new SequentialPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            mode: SequentialPartitionerMode.Decreasing,
            sequenceLength: 5,
            skipLength: 512,
            skipTrigger: 50
        );
    }
}