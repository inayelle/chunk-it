using System.Security.Cryptography;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Hashing;

public sealed class Sha512Hasher : IHasher
{
    public static Sha512Hasher Instance { get; } = new();

    public byte[] Hash(ReadOnlySpan<byte> bytes)
    {
        return SHA512.HashData(bytes);
    }
}