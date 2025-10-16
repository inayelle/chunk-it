using ChunkIt.Common;

namespace ChunkIt.Partitioning.Gear;

public sealed partial class GearPartitioner
{
    public override string ToString()
    {
        var builder = new DescriptionBuilder("gear");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .Build();
    }

    public bool Equals(GearPartitioner other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return _gearTable.Equals(other._gearTable) &&
               MinimumChunkSize == other.MinimumChunkSize &&
               AverageChunkSize == other.AverageChunkSize &&
               MaximumChunkSize == other.MaximumChunkSize;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is GearPartitioner other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            _gearTable,
            MinimumChunkSize,
            AverageChunkSize,
            MaximumChunkSize
        );
    }
}