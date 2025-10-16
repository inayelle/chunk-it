using ChunkIt.Common;

namespace ChunkIt.Partitioning.AsymmetricExtremum;

public sealed partial class AsymmetricExtremumPartitioner
{
    public override string ToString()
    {
        var builder = new DescriptionBuilder("asymmetric-extremum");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("window", _windowSize)
            .Build();
    }

    public bool Equals(AsymmetricExtremumPartitioner other)
    {
        if (other is null)
        {
            return false;
        }

        return other._windowSize == _windowSize &&
               other.MinimumChunkSize == MinimumChunkSize &&
               other.AverageChunkSize == AverageChunkSize &&
               other.MaximumChunkSize == MaximumChunkSize;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is AsymmetricExtremumPartitioner other && Equals(other);
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