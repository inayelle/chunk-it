namespace ChunkIt.Partitioning.Gear.Table;

public sealed partial class GearTable
{
    public static GearTable Random(ulong seed)
    {
        var table = new ulong[256];

        for (var i = 0; i < table.Length; i++)
        {
            table[i] = SplitMix64.NextUInt64(ref seed);
        }

        return new GearTable(table);
    }
}

file static class SplitMix64
{
    public static ulong NextUInt64(ref ulong seed)
    {
        unchecked
        {
            seed += 0x9E3779B97F4A7C15UL;

            var z = seed;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;

            return z ^ (z >> 31);
        }
    }
}