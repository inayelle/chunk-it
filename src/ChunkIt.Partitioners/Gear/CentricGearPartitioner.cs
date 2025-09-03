using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Partitioners.Gear;

public class CentricGearPartitioner : IPartitioner
{
    private readonly IGearTable _gearTable;

    private readonly int _normalizationLevel;
    private readonly ulong _mask;

    public int MinimumChunkSize { get; }
    public int AverageChunkSize { get; }
    public int MaximumChunkSize { get; }

    public CentricGearPartitioner(
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
        _mask = GenerateMasks(averageChunkSize, normalizationLevel);
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

        var leftCursor = Math.Max(MinimumChunkSize, mid - 1);
        var rightCursor = mid;

        var (leftPrint, rightPrint) = (0UL, 0UL);

        while (true)
        {
            var progressed = false;

            if (leftCursor >= MinimumChunkSize)
            {
                _gearTable.Fingerprint(ref leftPrint, buffer[leftCursor]);

                if ((leftPrint & _mask) == 0)
                {
                    return leftCursor;
                }

                leftCursor -= 1;
                progressed = true;
            }

            if (rightCursor < upper)
            {
                _gearTable.Fingerprint(ref rightPrint, buffer[rightCursor]);

                if ((rightPrint & _mask) == 0)
                {
                    return rightCursor;
                }

                rightCursor += 1;
                progressed = true;
            }

            if (!progressed)
            {
                return buffer.Length;
            }
        }
    }

    private static ulong GenerateMasks(int averageChunkSize, int normalizationLevel)
    {
        var kBase = (int)Math.Ceiling(Math.Log2(averageChunkSize));

        var k = Math.Max(1, kBase - normalizationLevel);

        var mask = (1UL << k) - 1;

        return mask;
    }

    public string Describe()
    {
        var builder = new DescriptionBuilder("centric-gear");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("norm_level", _normalizationLevel)
            .Build();
    }

    public override string ToString()
    {
        var builder = new DescriptionBuilder("centric-gear");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .Build();
    }
}