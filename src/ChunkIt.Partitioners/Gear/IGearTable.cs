namespace ChunkIt.Partitioners.Gear;

public interface IGearTable
{
    void Fingerprint(ref ulong fingerprint, byte value);
}