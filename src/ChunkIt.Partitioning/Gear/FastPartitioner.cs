using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Partitioning.Gear;

public sealed class FastPartitioner : IPartitioner
{
    private readonly GearTable _gearTable;

    private readonly int _normalizationLevel;
    private readonly ulong _strictMask;
    private readonly ulong _laxMask;

    public int MinimumChunkSize { get; }
    public int AverageChunkSize { get; }
    public int MaximumChunkSize { get; }

    public FastPartitioner(
        int minimumChunkSize,
        int averageChunkSize,
        int maximumChunkSize,
        int normalizationLevel,
        GearTable gearTable
    )
    {
        MinimumChunkSize = minimumChunkSize;
        AverageChunkSize = averageChunkSize;
        MaximumChunkSize = maximumChunkSize;

        _normalizationLevel = normalizationLevel;
        _gearTable = gearTable;

        (_strictMask, _laxMask) = GenerateMasks(averageChunkSize, normalizationLevel);
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

        var mid = Math.Min(buffer.Length, AverageChunkSize);
        var upper = buffer.Length;

        var fingerprint = 0UL;
        var cursor = MinimumChunkSize;

        for (; cursor < mid; cursor++)
        {
            _gearTable.Fingerprint(ref fingerprint, in buffer[cursor]);

            if ((fingerprint & _strictMask) == 0)
            {
                return cursor;
            }
        }

        for (; cursor < upper; cursor++)
        {
            _gearTable.Fingerprint(ref fingerprint, in buffer[cursor]);

            if ((fingerprint & _laxMask) == 0)
            {
                return cursor;
            }
        }

        return cursor;
    }

    private static (ulong StrictMask, ulong LaxMask) GenerateMasks(int averageChunkSize, int normalizationLevel)
    {
        var k = (int)Math.Ceiling(Math.Log2(averageChunkSize));

        var kStrict = Math.Min(63, k + normalizationLevel);
        var kLax = Math.Max(1, k - normalizationLevel);

        var strictMask = (1UL << kStrict) - 1;
        var laxMask = (1UL << kLax) - 1;

        return (strictMask, laxMask);
    }

    public override string ToString()
    {
        var builder = new DescriptionBuilder("fast");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("norm_level", _normalizationLevel)
            .Build();
    }
}