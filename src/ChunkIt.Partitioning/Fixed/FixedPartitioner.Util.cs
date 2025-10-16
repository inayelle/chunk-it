using ChunkIt.Common;

namespace ChunkIt.Partitioning.Fixed;

public sealed partial class FixedPartitioner
{
    public override string ToString()
    {
        var builder = new DescriptionBuilder("fixed");

        return builder
            .AddParameter("size", _chunkSize)
            .Build();
    }

    public bool Equals(FixedPartitioner other)
    {
        if (other is null)
        {
            return false;
        }

        return other._chunkSize == _chunkSize;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is FixedPartitioner other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _chunkSize.GetHashCode();
    }
}