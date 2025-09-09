using ChunkIt.Common.Abstractions;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class ChunkingContext
{
    private readonly List<Chunk> _chunks;
    private readonly HashSet<int> _progress;
    private readonly Action<int> _onProgress;

    private long _totalChunksLength;

    public IPartitioner Partitioner { get; }
    public SourceFile SourceFile { get; }

    public IReadOnlyList<Chunk> Chunks => _chunks;

    public ChunkingContext(IPartitioner partitioner, SourceFile sourceFile, Action<int> onProgress)
    {
        _chunks = new List<Chunk>(capacity: 100_000);
        _progress = new HashSet<int>(capacity: 100);
        _onProgress = onProgress;

        Partitioner = partitioner;
        SourceFile = sourceFile;
    }

    public void AddChunk(Chunk chunk)
    {
        _chunks.Add(chunk);

        _totalChunksLength += chunk.Length;

        var progress = (int)(_totalChunksLength / (float)SourceFile.Size * 100);

        if (_progress.Add(progress))
        {
            _onProgress.Invoke(progress);
        }
    }
}