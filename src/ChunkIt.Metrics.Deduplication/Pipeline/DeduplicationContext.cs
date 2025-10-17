using ChunkIt.Common.Abstractions;

namespace ChunkIt.Metrics.Deduplication.Pipeline;

public sealed class DeduplicationContext
{
    public delegate void OnProgressChanged(int progress);

    private readonly List<Chunk> _chunks;
    private readonly OnProgressChanged _onProgressChanged;

    private int _totalProgress;
    private long _totalChunksLength;

    public Input Input { get; }

    public IReadOnlyList<Chunk> Chunks => _chunks;

    public DeduplicationContext(
        Input input,
        OnProgressChanged onProgressChanged
    )
    {
        _chunks = new List<Chunk>(capacity: 1_000_000);
        _onProgressChanged = onProgressChanged;

        Input = input;
    }

    public void AddChunk(Chunk chunk)
    {
        _chunks.Add(chunk);

        _totalChunksLength += chunk.Length;

        var currentProgress = (int)(
            _totalChunksLength / (float)Input.SourceFile.Size * 100
        );

        if (currentProgress <= _totalProgress)
        {
            return;
        }

        _totalProgress = currentProgress;
        _onProgressChanged.Invoke(currentProgress);
    }
}