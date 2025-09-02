namespace ChunkIt.Abstractions;

public interface IHasher
{
    byte[] Hash(ReadOnlySpan<byte> bytes);
}