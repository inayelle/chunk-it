namespace ChunkIt.Common.Abstractions;

public interface IHasher
{
    byte[] Hash(ReadOnlySpan<byte> bytes);
}