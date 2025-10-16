using ChunkIt.Common;

namespace ChunkIt.Partitioning.RapidAsymmetricMaximum;

public sealed partial class RapidAsymmetricMaximumPartitioner
{
    public override string ToString()
    {
        var builder = new DescriptionBuilder("rapid-asymmetric-maximum");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("window", _windowSize)
            .Build();
    }

    public bool Equals(RapidAsymmetricMaximumPartitioner other)
    {
        if (other is null)
        {
            return false;
        }

        return _windowSize == other._windowSize &&
               MinimumChunkSize == other.MinimumChunkSize &&
               AverageChunkSize == other.AverageChunkSize &&
               MaximumChunkSize == other.MaximumChunkSize;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is RapidAsymmetricMaximumPartitioner other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            _windowSize,
            MinimumChunkSize,
            AverageChunkSize,
            MaximumChunkSize
        );
    }
}