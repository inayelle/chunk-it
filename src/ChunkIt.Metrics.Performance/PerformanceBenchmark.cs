using BenchmarkDotNet.Attributes;
using ChunkIt.Common;
using ChunkIt.Common.Abstractions;
using ChunkIt.Hashing;
using ChunkIt.Metrics.Inputs;

namespace ChunkIt.Metrics.Performance;

[Config(typeof(PerformanceBenchmarkConfig))]
public class PerformanceBenchmark
{
    private const int Kilobyte = 1024;
    private const int BufferSize = Kilobyte * 1000 * 4;

    private FileStream _sourceFileStream;

    private ChunkReader _chunkReader;

    [ParamsSource(typeof(InputsProvider), nameof(InputsProvider.Enumerate))]
    public Input Input { get; set; }

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
        _sourceFileStream = Input.SourceFile.OpenFileStream(BufferSize);

        _chunkReader = new ChunkReader(Input.Partitioner, NoneHasher.Instance, BufferSize);
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _sourceFileStream.Dispose();
    }
}