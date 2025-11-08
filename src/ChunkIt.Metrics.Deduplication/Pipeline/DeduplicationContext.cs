using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Metrics.Deduplication.Pipeline;

public sealed class DeduplicationContext
{
    private readonly IProgressReporter _progressReporter;
    private readonly List<Chunk> _chunks;

    private int _totalProgress;
    private long _totalChunksLength;

    public Input Input { get; }

    public IReadOnlyList<Chunk> Chunks => _chunks;

    public DeduplicationContext(Input input, IProgressReporter progressReporter)
    {
        _progressReporter = progressReporter;
        _chunks = new List<Chunk>(capacity: 1_000_000);

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
        _progressReporter.Report(Input, currentProgress);
    }
}