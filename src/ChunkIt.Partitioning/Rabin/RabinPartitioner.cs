using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Partitioning.Rabin;

public sealed class RabinPartitioner : IPartitioner
{
    private const ulong Prime = 153_191UL;
    private const ulong Mask = 0x00FF_FFFF_FFFFUL;
    private const ulong Polynomial = 0xBFE6_B8A5_BF37_8D83UL;

    private const int WindowSize = 16;
    private const int WindowMask = WindowSize - 1;
    private const int WindowSlideOffset = 64;

    private static readonly ulong[] OutMap = new ulong[256];
    private static readonly ulong[] Ir = new ulong[256];

    public int MinimumChunkSize { get; }
    public int AverageChunkSize { get; }
    public int MaximumChunkSize { get; }

    private readonly int _winSlidePos;
    private readonly ulong _cutMask;

    static RabinPartitioner()
    {
        var polyPow = 1UL;
        for (var i = 0; i < WindowSize; i++)
        {
            polyPow = (polyPow * Prime) & Mask;
        }

        for (var i = 0UL; i < 256; i++)
        {
            OutMap[i] = (i * polyPow) & Mask;
        }

        for (var i = 0; i < 256; i++)
        {
            var term = 1UL;
            var pow = 1UL;
            var val = 1UL;

            for (var j = 0; j < WindowSize; j++)
            {
                if ((term & Polynomial) != 0UL)
                {
                    val = (val + ((pow * (ulong)i) & Mask)) & Mask;
                }

                pow = (pow * Prime) & Mask;
                term <<= 1;
            }

            Ir[i] = val & Mask;
        }
    }

    public RabinPartitioner(int minimumChunkSize, int averageChunkSize, int maximumChunkSize)
    {
        MinimumChunkSize = minimumChunkSize;
        AverageChunkSize = averageChunkSize;
        MaximumChunkSize = maximumChunkSize;

        _winSlidePos = MinimumChunkSize - WindowSlideOffset;
        _cutMask = (ulong)(averageChunkSize - minimumChunkSize - 1);
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

        var cursor = Math.Min(_winSlidePos, buffer.Length);

        var window = (Span<byte>)stackalloc byte[WindowSize];
        var windowIndex = 0;

        var hash = 0UL;

        while (cursor < buffer.Length)
        {
            var currentByte = buffer[cursor];
            var outByte = window[windowIndex];
            var pushedOut = OutMap[outByte];

            unchecked
            {
                hash = (hash * Prime) & Mask;
                hash = (hash + currentByte) & Mask;
                hash = (hash - pushedOut) & Mask;
            }

            window[windowIndex] = currentByte;
            windowIndex = (windowIndex + 1) & WindowMask;

            if (cursor >= MinimumChunkSize)
            {
                var checksum = hash ^ Ir[outByte];

                if ((checksum & _cutMask) == 0UL)
                {
                    return cursor;
                }
            }

            cursor += 1;
        }

        return buffer.Length;
    }

    public override string ToString()
    {
        var builder = new DescriptionBuilder("rabin");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("window", WindowSize)
            .AddParameter("cut_mask", _cutMask)
            .Build();
    }
}