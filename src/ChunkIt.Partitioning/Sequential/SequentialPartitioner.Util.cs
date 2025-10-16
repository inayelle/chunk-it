using ChunkIt.Common;

namespace ChunkIt.Partitioning.Sequential;

public sealed partial class SequentialPartitioner
{
    public override string ToString()
    {
        var modeString = _mode switch
        {
            SequentialPartitionerMode.Increasing => "incr",
            SequentialPartitionerMode.Decreasing => "decr",
            _ => throw new ArgumentOutOfRangeException(nameof(_mode)),
        };

        var builder = new DescriptionBuilder($"seq-{modeString}");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("seqLen", _sequenceLength)
            .AddParameter("skipTrig", _skipTrigger)
            .AddParameter("skipLen", _skipLength)
            .Build();
    }

    public bool Equals(SequentialPartitioner other)
    {
        if (other is null)
        {
            return false;
        }

        return _sequenceLength == other._sequenceLength &&
               _skipLength == other._skipLength &&
               _skipTrigger == other._skipTrigger &&
               _mode == other._mode &&
               MinimumChunkSize == other.MinimumChunkSize &&
               AverageChunkSize == other.AverageChunkSize &&
               MaximumChunkSize == other.MaximumChunkSize;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is SequentialPartitioner other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            _sequenceLength,
            _skipLength,
            _skipTrigger,
            (int)_mode,
            MinimumChunkSize,
            AverageChunkSize,
            MaximumChunkSize
        );
    }
}