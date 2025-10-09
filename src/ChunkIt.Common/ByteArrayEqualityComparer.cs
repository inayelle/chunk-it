namespace ChunkIt.Common;

public sealed class ByteArrayEqualityComparer : IEqualityComparer<byte[]>
{
    public static ByteArrayEqualityComparer Instance { get; } = new();

    public bool Equals(byte[] x, byte[] y)
    {
        if (x is null && y is null)
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        return x.SequenceEqual(y);
    }

    public int GetHashCode(byte[] obj)
    {
        return obj.Aggregate(0, HashCode.Combine);
    }
}