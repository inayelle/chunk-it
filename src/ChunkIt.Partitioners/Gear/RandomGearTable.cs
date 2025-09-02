using System.Runtime.CompilerServices;
using ChunkIt.Common.Extensions;

namespace ChunkIt.Partitioners.Gear;

public class RandomGearTable : IGearTable
{
    private const int Capacity = 256;

    private readonly ulong[] _table;

    public RandomGearTable(Random random)
    {
        _table = new ulong[Capacity];

        var seed = random.NextUInt64();

        for (var i = 0; i < _table.Length; i++)
        {
            _table[i] = SplitMix64(ref seed);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Fingerprint(ref ulong fingerprint, byte value)
    {
        unchecked
        {
            fingerprint = (fingerprint << 1) + _table[value];
        }
    }

    private static ulong SplitMix64(ref ulong seed)
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