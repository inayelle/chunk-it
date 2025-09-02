using AnyKit.Pipelines;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class WriteChunksPipe : IChunkingPipe
{
    public async Task<ChunkingReport> Invoke(
        ChunkingContext context,
        AsyncPipeline<ChunkingContext, ChunkingReport> next
    )
    {
        var formatter = new Tababular.TableFormatter();

        var objects = context.Chunks.Select(chunk => new
            {
                Id = chunk.Id,
                Offset = chunk.Offset,
                Length = chunk.Length,
                Hash = chunk.HashString,
            }
        );

        var chunksTableText = formatter.FormatObjects(objects);

        var chunksFilePath = SandboxRuntime.Instance.GetChunksFilePath(
            context.Partitioner.ToString(),
            context.SourceFilePath
        );

        await File.WriteAllTextAsync(chunksFilePath, chunksTableText);

        return await next(context);
    }
}