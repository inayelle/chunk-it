using ChunkIt.Abstractions;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class ChunkingContext
{
    private readonly List<Chunk> _chunks;

    public IPartitioner Partitioner { get; }

    public string SourceFilePath { get; }

    public IReadOnlyList<Chunk> Chunks => _chunks;

    public ChunkingContext(IPartitioner partitioner, string sourceFilePath)
    {
        _chunks = new List<Chunk>(capacity: 100_000);

        Partitioner = partitioner;
        SourceFilePath = sourceFilePath;
    }

    public void AddChunk(Chunk chunk)
    {
        _chunks.Add(chunk);
    }
}