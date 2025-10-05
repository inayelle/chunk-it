using ChunkIt.Common.Abstractions;

namespace ChunkIt.Sandbox.Chunking;

internal sealed class ChunkingContext
{
    public delegate void OnProgressChanged(int progress);

    private readonly List<Chunk> _chunks;
    private readonly OnProgressChanged _onProgressChanged;

    private int _totalProgress;
    private long _totalChunksLength;

    public IPartitioner Partitioner { get; }
    public SourceFile SourceFile { get; }

    public IReadOnlyList<Chunk> Chunks => _chunks;

    public ChunkingContext(IPartitioner partitioner, SourceFile sourceFile, OnProgressChanged onProgressChanged)
    {
        _chunks = new List<Chunk>(capacity: 100_000);
        _onProgressChanged = onProgressChanged;

        Partitioner = partitioner;
        SourceFile = sourceFile;
    }

    public void AddChunk(Chunk chunk)
    {
        _chunks.Add(chunk);

        _totalChunksLength += chunk.Length;

        var currentProgress = (int)(_totalChunksLength / (float)SourceFile.Size * 100);

        if (currentProgress <= _totalProgress)
        {
            return;
        }

        _totalProgress = currentProgress;
        _onProgressChanged.Invoke(currentProgress);
    }
}