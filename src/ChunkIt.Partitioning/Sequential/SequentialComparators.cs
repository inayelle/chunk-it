namespace ChunkIt.Partitioning.Sequential;

internal static class SequentialComparators
{
    public static bool IsIncreasing(byte previousByte, byte currentByte)
    {
        return previousByte < currentByte;
    }

    public static bool IsDecreasing(byte previousByte, byte currentByte)
    {
        return previousByte > currentByte;
    }

    public static bool IsIncreasing(byte previousByte, byte currentByte, bool strict)
    {
        return strict switch
        {
            true => previousByte < currentByte,
            false => previousByte <= currentByte,
        };
    }

    public static bool IsDecreasing(byte previousByte, byte currentByte, bool strict)
    {
        return strict switch
        {
            true => previousByte > currentByte,
            false => previousByte >= currentByte,
        };
    }
}