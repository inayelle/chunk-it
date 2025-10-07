using ChunkIt.Common.Abstractions;

namespace ChunkIt.Hashing;

public sealed class Xx128Hasher : IHasher
{
    public static Xx128Hasher Instance { get; } = new();

    public byte[] Hash(ReadOnlySpan<byte> bytes)
    {
        return System.IO.Hashing.XxHash128.Hash(bytes);
    }
}