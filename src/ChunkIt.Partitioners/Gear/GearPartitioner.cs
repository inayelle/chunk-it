using ChunkIt.Abstractions;

namespace ChunkIt.Partitioners.Gear;

public class GearPartitioner : IPartitioner
{
    private readonly IGearTable _gearTable;

    private readonly int _normalizationLevel;
    private readonly ulong _strictMask;
    private readonly ulong _laxMask;

    public int MinimumChunkSize { get; }
    public int AverageChunkSize { get; }
    public int MaximumChunkSize { get; }

    public GearPartitioner(
        IGearTable gearTable,
        int minimumChunkSize,
        int averageChunkSize,
        int maximumChunkSize,
        int normalizationLevel
    )
    {
        _gearTable = gearTable;

        MinimumChunkSize = minimumChunkSize;
        AverageChunkSize = averageChunkSize;
        MaximumChunkSize = maximumChunkSize;
        _normalizationLevel = normalizationLevel;

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

        var hash = 0UL;
        var cursor = MinimumChunkSize;

        var upper = Math.Min(buffer.Length, MaximumChunkSize);
        var mid = Math.Min(upper, AverageChunkSize);

        for (; cursor < mid; cursor++)
        {
            _gearTable.Fingerprint(ref hash, buffer[cursor]);

            if ((hash & _strictMask) == 0)
            {
                return cursor;
            }
        }

        for (; cursor < upper; cursor++)
        {
            _gearTable.Fingerprint(ref hash, buffer[cursor]);

            if ((hash & _laxMask) == 0)
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

    public string Describe()
    {
        var builder = new DescriptionBuilder("gear");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("norm_level", _normalizationLevel)
            .Build();
    }

    public override string ToString()
    {
        return "gear";
    }
}