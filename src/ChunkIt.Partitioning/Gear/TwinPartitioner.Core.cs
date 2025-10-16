using ChunkIt.Common.Abstractions;
using ChunkIt.Partitioning.Gear.Table;

namespace ChunkIt.Partitioning.Gear;

public sealed partial class TwinPartitioner
    : IPartitioner,
      IEquatable<TwinPartitioner>
{
    private readonly GearTable _leftGearTable;
    private readonly GearTable _rightGearTable;

    private readonly int _normalizationLevel;
    private readonly ulong _mask;

    public int MinimumChunkSize { get; }
    public int AverageChunkSize { get; }
    public int MaximumChunkSize { get; }

    public TwinPartitioner(
        int minimumChunkSize,
        int averageChunkSize,
        int maximumChunkSize,
        int normalizationLevel,
        GearTable gearTable
    ) : this(
        minimumChunkSize,
        averageChunkSize,
        maximumChunkSize,
        normalizationLevel,
        gearTable,
        gearTable
    )
    {
    }

    public TwinPartitioner(
        int minimumChunkSize,
        int averageChunkSize,
        int maximumChunkSize,
        int normalizationLevel,
        GearTable leftGearTable,
        GearTable rightGearTable
    )
    {
        MinimumChunkSize = minimumChunkSize;
        AverageChunkSize = averageChunkSize;
        MaximumChunkSize = maximumChunkSize;

        _leftGearTable = leftGearTable;
        _rightGearTable = rightGearTable;

        _normalizationLevel = normalizationLevel;
        _mask = GenerateMask(averageChunkSize, normalizationLevel);
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

        var (leftFingerprint, rightFingerprint) = (0UL, 0UL);

        var leftCursor = Math.Max(MinimumChunkSize, mid - 1);
        var rightCursor = mid;

        var alternative = (Match: UInt64.MaxValue, Cursor: 0);

        while (true)
        {
            var progressed = false;

            if (leftCursor >= MinimumChunkSize)
            {
                _leftGearTable.Fingerprint(ref leftFingerprint, in buffer[leftCursor]);

                var match = leftFingerprint & _mask;

                if (match == 0)
                {
                    return leftCursor;
                }

                if (match < alternative.Match)
                {
                    alternative = (match, leftCursor);
                }

                leftCursor -= 1;
                progressed = true;
            }

            if (rightCursor < upper)
            {
                _rightGearTable.Fingerprint(ref rightFingerprint, in buffer[rightCursor]);

                var match = rightFingerprint & _mask;

                if (match == 0)
                {
                    return rightCursor;
                }

                if (match < alternative.Match)
                {
                    alternative = (match, rightCursor);
                }

                rightCursor += 1;
                progressed = true;
            }

            if (!progressed)
            {
                break;
            }
        }

        return alternative.Cursor != 0
            ? alternative.Cursor
            : buffer.Length;
    }

    private static ulong GenerateMask(int averageChunkSize, int normalizationLevel)
    {
        var kBase = (int)Math.Ceiling(Math.Log2(averageChunkSize));

        var k = Math.Max(1, kBase - normalizationLevel);

        var mask = (1UL << k) - 1;

        return mask;
    }
}