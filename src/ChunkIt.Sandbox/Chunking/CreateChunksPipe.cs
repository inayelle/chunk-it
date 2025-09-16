using System.Diagnostics;
using AnyKit.Pipelines;
using ChunkIt.Common;
using ChunkIt.Hashers;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class CreateChunksPipe : IChunkingPipe
{
    private const int Kilobyte = 1024;
    private const int BufferSize = Kilobyte * 1000 * 4;

    public async Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    )
    {
        await using var sourceFileStream = context.SourceFile.OpenFileStream(BufferSize);

        var chunkReader = new ChunkReader(
            partitioner: context.Partitioner,
            hasher: Sha256Hasher.Instance,
            bufferSize: BufferSize
        );

        var stopwatch = Stopwatch.GetTimestamp();

        await foreach (var chunk in chunkReader.ReadChunksAsync(sourceFileStream))
        {
            context.AddChunk(chunk);
        }

        var elapsed = Stopwatch.GetElapsedTime(stopwatch);

        var report = await next(context);

        report.Elapsed = elapsed;
        report.TotalChunks = context.Chunks.Count;
        report.AverageChunkSize = (int)Math.Ceiling(context.Chunks.Average(chunk => chunk.Length));

        return report;
    }
}