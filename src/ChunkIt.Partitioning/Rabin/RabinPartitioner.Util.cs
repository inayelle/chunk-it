using ChunkIt.Common;

namespace ChunkIt.Partitioning.Rabin;

public sealed partial class RabinPartitioner
{
    public override string ToString()
    {
        var builder = new DescriptionBuilder("rabin");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("window", WindowSize)
            .AddParameter("cut_mask", _cutMask)
            .Build();
    }

    public bool Equals(RabinPartitioner other)
    {
        if (other is null)
        {
            return false;
        }

        return other._winSlidePos == _winSlidePos &&
               other._cutMask == _cutMask &&
               other.MinimumChunkSize == MinimumChunkSize &&
               other.AverageChunkSize == AverageChunkSize &&
               other.MaximumChunkSize == MaximumChunkSize;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is RabinPartitioner other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            _winSlidePos,
            _cutMask,
            MinimumChunkSize,
            AverageChunkSize,
            MaximumChunkSize
        );
    }
}