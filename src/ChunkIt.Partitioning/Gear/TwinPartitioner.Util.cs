using ChunkIt.Common;

namespace ChunkIt.Partitioning.Gear;

public sealed partial class TwinPartitioner
{
    public override string ToString()
    {
        var gearMode = _leftGearTable.Equals(_rightGearTable)
            ? "mono"
            : "duo";

        var builder = new DescriptionBuilder($"twin-{gearMode}");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("norm_level", _normalizationLevel)
            .Build();
    }

    public bool Equals(TwinPartitioner other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return _leftGearTable.Equals(other._leftGearTable) &&
               _rightGearTable.Equals(other._rightGearTable) &&
               _normalizationLevel == other._normalizationLevel &&
               MinimumChunkSize == other.MinimumChunkSize &&
               AverageChunkSize == other.AverageChunkSize &&
               MaximumChunkSize == other.MaximumChunkSize;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is TwinPartitioner other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            _leftGearTable,
            _rightGearTable,
            _normalizationLevel,
            MinimumChunkSize,
            AverageChunkSize,
            MaximumChunkSize
        );
    }
}