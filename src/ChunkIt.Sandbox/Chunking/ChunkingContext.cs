using ChunkIt.Common.Abstractions;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class ChunkingContext
{
    private readonly List<Chunk> _chunks;

    public IPartitioner Partitioner { get; }
    public SourceFile SourceFile { get; }

    public IReadOnlyList<Chunk> Chunks => _chunks;

    public ChunkingContext(IPartitioner partitioner, SourceFile sourceFile)
    {
        _chunks = new List<Chunk>(capacity: 100_000);

        Partitioner = partitioner;
        SourceFile = sourceFile;
    }

    public void AddChunk(Chunk chunk)
    {
        _chunks.Add(chunk);
    }
}