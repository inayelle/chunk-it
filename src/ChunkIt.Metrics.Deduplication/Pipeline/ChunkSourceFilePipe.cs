using AnyKit.Pipelines;
using ChunkIt.Common;
using ChunkIt.Hashing;

namespace ChunkIt.Metrics.Deduplication.Pipeline;

internal sealed class ChunkSourceFilePipe : IDeduplicationPipe
{
    private const int Kilobyte = 1024;
    private const int BufferSize = Kilobyte * 1000 * 4;

    public async Task<DeduplicationReport> Invoke(
        DeduplicationContext context,
        AsyncPipeline<DeduplicationContext, DeduplicationReport> next
    )
    {
        await using var sourceFileStream = context.Input.SourceFile.OpenFileStream(BufferSize);

        var chunkReader = new ChunkReader(
            partitioner: context.Input.Partitioner,
            hasher: Sha256Hasher.Instance,
            bufferSize: BufferSize
        );

        await foreach (var chunk in chunkReader.ReadChunksAsync(sourceFileStream))
        {
            context.AddChunk(chunk);
        }

        return await next(context);
    }
}