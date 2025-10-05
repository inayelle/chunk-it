using System.Runtime.CompilerServices;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Hashing;

public sealed class NoneHasher : IHasher
{
    public static NoneHasher Instance { get; } = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte[] Hash(ReadOnlySpan<byte> bytes)
    {
        return [];
    }
}