namespace ChunkIt.Partitioning.Gear.Table;

public sealed partial class GearTable
{
    public bool Equals(GearTable other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return _table.SequenceEqual(other._table);
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is GearTable other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _table.Aggregate(0, HashCode.Combine);
    }
}