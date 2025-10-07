using ChunkIt.Common.Abstractions;

namespace ChunkIt.Hashing;

public sealed class Blake3Hasher : IHasher
{
    public static Blake3Hasher Instance { get; } = new();

    public byte[] Hash(ReadOnlySpan<byte> bytes)
    {
        var hash = new byte[32];

        Blake3.Hasher.Hash(bytes, hash.AsSpan());

        return hash;
    }
}