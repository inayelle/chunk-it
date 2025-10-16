using ChunkIt.Common;

namespace ChunkIt.Partitioning.Gear;

public sealed partial class FastPartitioner
{
    public override string ToString()
    {
        var builder = new DescriptionBuilder("fast");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("norm_level", _normalizationLevel)
            .Build();
    }

    public bool Equals(FastPartitioner other)
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
               _normalizationLevel == other._normalizationLevel &&
               MinimumChunkSize == other.MinimumChunkSize &&
               AverageChunkSize == other.AverageChunkSize &&
               MaximumChunkSize == other.MaximumChunkSize;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is FastPartitioner other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            _gearTable,
            _normalizationLevel,
            MinimumChunkSize,
            AverageChunkSize,
            MaximumChunkSize
        );
    }
}