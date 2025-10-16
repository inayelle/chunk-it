using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Partitioning.Gear;

public sealed class GearPartitioner : IPartitioner
{
    private readonly GearTable _gearTable;

    private readonly ulong _mask;

    public int MinimumChunkSize { get; }
    public int AverageChunkSize { get; }
    public int MaximumChunkSize { get; }

    public GearPartitioner(
        int minimumChunkSize,
        int averageChunkSize,
        int maximumChunkSize,
        GearTable gearTable
    )
    {
        MinimumChunkSize = minimumChunkSize;
        AverageChunkSize = averageChunkSize;
        MaximumChunkSize = maximumChunkSize;

        _gearTable = gearTable;

        _mask = GenerateMask(averageChunkSize);
    }

    public int FindChunkLength(ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length <= MinimumChunkSize)
        {
            return buffer.Length;
        }

        if (buffer.Length > MaximumChunkSize)
        {
            buffer = buffer.Slice(start: 0, length: MaximumChunkSize);
        }

        var fingerprint = 0UL;

        for (var cursor = 0; cursor < buffer.Length; cursor++)
        {
            _gearTable.Fingerprint(ref fingerprint, in buffer[cursor]);

            if ((fingerprint & _mask) == 0 && cursor >= MinimumChunkSize)
            {
                return cursor;
            }
        }

        return buffer.Length;
    }

    private static ulong GenerateMask(int averageChunkSize)
    {
        var k = (int)Math.Ceiling(Math.Log2(averageChunkSize));

        var mask = (1UL << k) - 1;

        return mask;
    }

    public override string ToString()
    {
        var builder = new DescriptionBuilder("gear");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .Build();
    }
}