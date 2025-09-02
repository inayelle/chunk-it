using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using ChunkIt.Abstractions;

namespace ChunkIt.Hashers;

public sealed class Sha256Hasher : IHasher
{
    public static Sha256Hasher Instance { get; } = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte[] Hash(ReadOnlySpan<byte> bytes)
    {
        return SHA256.HashData(bytes);
    }
}