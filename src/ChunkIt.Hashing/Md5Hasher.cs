using System.Security.Cryptography;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Hashing;

public sealed class Md5Hasher : IHasher
{
    public static Md5Hasher Instance { get; } = new();

    public byte[] Hash(ReadOnlySpan<byte> bytes)
    {
        return MD5.HashData(bytes);
    }
}