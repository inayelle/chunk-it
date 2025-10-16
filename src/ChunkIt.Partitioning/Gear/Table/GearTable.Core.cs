using System.Runtime.CompilerServices;

namespace ChunkIt.Partitioning.Gear.Table;

public sealed partial class GearTable : IEquatable<GearTable>
{
    private readonly ulong[] _table;

    private GearTable(ulong[] table)
    {
        _table = table;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Fingerprint(ref ulong fingerprint, ref readonly byte value)
    {
        unchecked
        {
            fingerprint = (fingerprint << 1) + _table[value];
        }
    }
}