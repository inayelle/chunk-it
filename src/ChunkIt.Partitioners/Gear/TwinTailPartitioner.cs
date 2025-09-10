using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Partitioners.Gear;

public class TwinTailPartitioner : IPartitioner
{
    private readonly GearTable _leftGearTable;
    private readonly GearTable _rightGearTable;

    private readonly int _normalizationLevel;
    private readonly ulong _mask;

    public int MinimumChunkSize { get; }
    public int AverageChunkSize { get; }
    public int MaximumChunkSize { get; }

    public TwinTailPartitioner(
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

    public TwinTailPartitioner(
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

        return FindChunkLength(buffer, _mask);
    }

    private int FindChunkLength(ReadOnlySpan<byte> buffer, ulong mask)
    {
        var mid = Math.Min(buffer.Length, AverageChunkSize);
        var upper = buffer.Length;

        while (mask > 0)
        {
            var (leftPrint, rightPrint) = (0UL, 0UL);

            var leftCursor = Math.Max(MinimumChunkSize, mid - 1);
            var rightCursor = mid;

            while (true)
            {
                var progressed = false;

                if (leftCursor >= MinimumChunkSize)
                {
                    _leftGearTable.Fingerprint(ref leftPrint, buffer[leftCursor]);

                    if ((leftPrint & mask) == 0)
                    {
                        return leftCursor;
                    }

                    leftCursor -= 1;
                    progressed = true;
                }

                if (rightCursor < upper)
                {
                    _rightGearTable.Fingerprint(ref rightPrint, buffer[rightCursor]);

                    if ((rightPrint & mask) == 0)
                    {
                        return rightCursor;
                    }

                    rightCursor += 1;
                    progressed = true;
                }

                if (!progressed)
                {
                    break;
                }
            }

            mask >>= 1;
        }

        return buffer.Length;
    }

    private static ulong GenerateMask(int averageChunkSize, int normalizationLevel)
    {
        var kBase = (int)Math.Ceiling(Math.Log2(averageChunkSize));

        var k = Math.Max(1, kBase - normalizationLevel);

        var mask = (1UL << k) - 1;

        return mask;
    }

    public override string ToString()
    {
        var gearMode = _leftGearTable == _rightGearTable ? "single" : "double";

        var builder = new DescriptionBuilder($"twin-tail-{gearMode}");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("norm_level", _normalizationLevel)
            .Build();
    }
}