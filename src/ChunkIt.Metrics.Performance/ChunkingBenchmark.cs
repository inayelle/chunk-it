using BenchmarkDotNet.Attributes;
using ChunkIt.Common;
using ChunkIt.Common.Abstractions;
using ChunkIt.Hashing;

namespace ChunkIt.Metrics.Performance;

[Config(typeof(ChunkingBenchmarkConfig))]
public class ChunkingBenchmark
{
    private const int Kilobyte = 1024;
    private const int BufferSize = Kilobyte * 1000 * 4;

    private FileStream _sourceFileStream;

    private ChunkReader _chunkReader;

    [ParamsSource(typeof(SourceFiles), nameof(SourceFiles.Enumerate))]
    public SourceFile SourceFile { get; set; }

    [ParamsSource(typeof(Partitioners), nameof(Partitioners.Enumerate))]
    public IPartitioner Partitioner { get; set; }

    [Benchmark]
    public async Task<ulong> Run()
    {
        _sourceFileStream.Position = 0;

        ulong length = 0;

        await foreach (var chunkLength in _chunkReader.ReadChunkLengthsAsync(_sourceFileStream).ConfigureAwait(false))
        {
            length += (ulong)chunkLength;
        }

        return length;
    }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _sourceFileStream = SourceFile.OpenFileStream(BufferSize);

        _chunkReader = new ChunkReader(Partitioner, NoneHasher.Instance, BufferSize);
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _sourceFileStream.Dispose();
    }
}