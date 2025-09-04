using AnyKit.Pipelines;
using ChunkIt.Common;
using ChunkIt.Hashers;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class CreateChunksPipe : IChunkingPipe
{
    private const int BufferSize = 100 * 1024;

    public async Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    )
    {
        await using var sourceFileStream = new FileStream(
            context.SourceFile.Path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            BufferSize
        );

        var chunkReader = new ChunkReader(
            partitioner: context.Partitioner,
            hasher: Sha256Hasher.Instance,
            BufferSize
        );

        await foreach (var chunk in chunkReader.ReadAsync(sourceFileStream))
        {
            context.AddChunk(chunk);
        }

        var report = await next(context);

        report.TotalChunks = context.Chunks.Count;
        report.AverageChunkSize = (int)Math.Ceiling(context.Chunks.Average(chunk => chunk.Length));

        return report;
    }
}