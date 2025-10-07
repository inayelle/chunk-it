using System.Numerics;
using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Partitioning.Gear;

public class SlidingGearPartitioner : IPartitioner
{
    private readonly GearTable _gearTable;

    private readonly int _normalizationLevel;
    private readonly ulong[] _masks;
    private const int Block = 64;

    public int MinimumChunkSize { get; }
    public int AverageChunkSize { get; }
    public int MaximumChunkSize { get; }

    public SlidingGearPartitioner(
        GearTable gearTable,
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

        _masks = BuildSlidingMasks(minimumChunkSize, averageChunkSize, maximumChunkSize, normalizationLevel);
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

        var upper = Math.Min(buffer.Length, MaximumChunkSize);
        var fp = 0UL;

        var cursor = MinimumChunkSize;
        while (cursor < upper)
        {
            _gearTable.Fingerprint(ref fp, in buffer[cursor]);

            var idx = Math.Min((cursor - MinimumChunkSize) / Block, _masks.Length - 1);
            var mask = _masks[idx];

            if ((fp & mask) == 0)
            {
                return cursor;
            }

            cursor++;
        }

        return upper;
    }

    private static ulong[] BuildSlidingMasks(int min, int avg, int max, int norm)
    {
        var span = max - min;
        var blocks = Math.Max(1, (span + Block - 1) / Block);
        var masks = new ulong[blocks];

        var kBase = Math.Max(1, (int)Math.Ceiling(Math.Log2(Math.Max(2, avg))));
        var kMax = Math.Min(63, kBase + norm);
        var kMin = Math.Max(1, kBase - norm);

        for (var j = 0; j < blocks; j++)
        {
            var t = blocks == 1 ? 1.0 : (double)j / (blocks - 1); // 0..1
            var kf = kMax + (kMin - kMax) * t;
            var k = (int)Math.Round(kf);
            if (k >= 64)
            {
                masks[j] = ~0UL;
            }
            else
            {
                masks[j] = (1UL << k) - 1UL;
            }
        }

        var delta = avg - min;
        var blocksToAvg = Math.Min(blocks, Math.Max(0, (delta + Block - 1) / Block));
        var S = 0.0;
        for (var j = 0; j < blocksToAvg; j++)
        {
            var k = BitOperations.TrailingZeroCount(masks[j] + 1);
            S += Block * Math.Pow(2.0, -k);
        }

        if (S < 1.0 && kMin > 1)
        {
            for (var j = 0; j < blocks; j++)
            {
                var k = BitOperations.TrailingZeroCount(masks[j] + 1);
                if (k > 1)
                {
                    k -= 1;
                    masks[j] = (1UL << k) - 1UL;
                }
            }
        }
        else if (S > 1.5 && kMax < 63)
        {
            for (var j = 0; j < blocks; j++)
            {
                var k = BitOperations.TrailingZeroCount(masks[j] + 1);
                if (k < 63)
                {
                    k += 1;
                    masks[j] = (1UL << k) - 1UL;
                }
            }
        }

        return masks;
    }

    public override string ToString()
    {
        var builder = new DescriptionBuilder("sliding-gear");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("norm_level", _normalizationLevel)
            .Build();
    }
}