namespace ChunkIt.Common.Extensions;

public static class RandomExtensions
{
    public static ulong NextUInt64(this Random random)
    {
        var highBits = random.NextInt64();
        var lowBits = random.NextInt64();

        return ((ulong)highBits << 32) | (ulong)lowBits;
    }
}