using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Partitioners.Sequential;

public class SequentialPartitioner : IPartitioner
{
    private delegate bool Comparator(byte previousByte, byte currentByte);

    private readonly int _sequenceLength;
    private readonly int _skipLength;
    private readonly int _skipTrigger;

    private readonly SequentialPartitionerMode _mode;

    private readonly Comparator _comparator;

    public int MinimumChunkSize { get; }
    public int AverageChunkSize { get; }
    public int MaximumChunkSize { get; }

    public SequentialPartitioner(
        SequentialPartitionerMode mode,
        int minimumChunkSize,
        int averageChunkSize,
        int maximumChunkSize,
        int sequenceLength,
        int skipLength,
        int skipTrigger
    )
    {
        MinimumChunkSize = minimumChunkSize;
        AverageChunkSize = averageChunkSize;
        MaximumChunkSize = maximumChunkSize;

        _sequenceLength = sequenceLength;
        _skipLength = skipLength;
        _skipTrigger = skipTrigger;

        _mode = mode;
        _comparator = mode switch
        {
            SequentialPartitionerMode.Increasing => SequentialComparators.IsIncreasing,
            SequentialPartitionerMode.Decreasing => SequentialComparators.IsDecreasing,
            _ => throw new ArgumentOutOfRangeException(nameof(mode)),
        };
    }

    public int FindChunkLength(ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length <= MinimumChunkSize)
        {
            return buffer.Length;
        }

        if (buffer.Length > MaximumChunkSize)
        {
            buffer = buffer.Slice(start: 0, length: MaximumChunkSize);
        }

        var window = new SequentialWindow();
        var cursor = Math.Max(
            MinimumChunkSize - _sequenceLength + 1,
            1
        );

        while (cursor < buffer.Length)
        {
            if (_comparator(buffer[cursor - 1], buffer[cursor]))
            {
                window.IncrementSequence();

                if (window.SequenceLength == _sequenceLength)
                {
                    return cursor;
                }
            }
            else
            {
                window.IncrementChaos();

                if (window.ChaosLength == _skipTrigger)
                {
                    cursor += _skipLength;
                    window.Reset();

                    continue;
                }
            }

            cursor += 1;
        }

        return Math.Min(cursor, buffer.Length);
    }

    public override string ToString()
    {
        var modeString = _mode switch
        {
            SequentialPartitionerMode.Increasing => "incr",
            SequentialPartitionerMode.Decreasing => "decr",
            _ => throw new ArgumentOutOfRangeException(nameof(_mode)),
        };

        var builder = new DescriptionBuilder($"adapt-seq-{modeString}");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("seqLength", _sequenceLength)
            .AddParameter("skipTrigger", _skipTrigger)
            .AddParameter("skipLength", _skipLength)
            .Build();
    }
}