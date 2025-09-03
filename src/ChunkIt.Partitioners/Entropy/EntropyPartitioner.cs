using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Partitioners.Entropy;

public class EntropyPartitioner : IPartitioner
{
    private readonly int _windowSize;
    private readonly int _lowThreshold;
    private readonly int _highThreshold;

    private readonly int[] _log2CountQ16;
    private readonly int[] _log2LengthQ16;

    public int MinimumChunkSize { get; }
    public int AverageChunkSize { get; }
    public int MaximumChunkSize { get; }

    public EntropyPartitioner(
        int minimumChunkSize,
        int averageChunkSize,
        int maximumChunkSize,
        int windowSize,
        double lowThresholdBits,
        double highThresholdBits
    )
    {
        MinimumChunkSize = minimumChunkSize;
        AverageChunkSize = averageChunkSize;
        MaximumChunkSize = maximumChunkSize;

        _windowSize = windowSize;

        _lowThreshold = (int)Math.Round(lowThresholdBits * 65536);
        _highThreshold = (int)Math.Round(highThresholdBits * 65536);

        _log2CountQ16 = new int[_windowSize + 1];
        _log2LengthQ16 = new int[_windowSize + 1];

        for (var index = 1; index <= _windowSize; index++)
        {
            _log2CountQ16[index] = (int)Math.Round(Math.Log2(index) * 65536.0);
            _log2LengthQ16[index] = (int)Math.Round(Math.Log2(index) * 65536.0);
        }
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
        var mid = Math.Min(upper, AverageChunkSize);

        var cursor = MinimumChunkSize;

        Span<int> hist = stackalloc int[16];
        Span<byte> ring = stackalloc byte[_windowSize];

        var start = Math.Max(0, cursor - _windowSize);
        var L = cursor - start;
        var sumTermQ16 = 0L;

        var head = 0;
        for (var i = start; i < cursor; i++)
        {
            var b = buffer[i];
            var bucket = b >> 4;
            ring[head++] = (byte)bucket;
            var cOld = hist[bucket];
            var cNew = cOld + 1;
            sumTermQ16 += (long)cNew * _log2CountQ16[cNew] - (long)cOld * _log2CountQ16[cOld];
            hist[bucket] = cNew;
        }

        if (head == _windowSize)
        {
            head = 0;
        }

        while (cursor < mid)
        {
            var b = buffer[cursor];
            var bucketIn = b >> 4;

            if (L == _windowSize)
            {
                int bucketOut = ring[head];
                var cOld = hist[bucketOut];
                var cNew = cOld - 1;
                sumTermQ16 += (long)cNew * _log2CountQ16[cNew] - (long)cOld * _log2CountQ16[cOld];
                hist[bucketOut] = cNew;
            }
            else
            {
                L++;
            }

            ring[head] = (byte)bucketIn;
            head++;
            if (head == _windowSize)
            {
                head = 0;
            }

            {
                var cOld = hist[bucketIn];
                var cNew = cOld + 1;
                sumTermQ16 += (long)cNew * _log2CountQ16[cNew] - (long)cOld * _log2CountQ16[cOld];
                hist[bucketIn] = cNew;
            }

            var hQ16 = CalculateEntropy(L, sumTermQ16);
            if (hQ16 <= _lowThreshold)
            {
                return cursor;
            }

            cursor++;
        }

        // ---- Фаза 2: [mid .. upper) із м’якшим порогом ----
        while (cursor < upper)
        {
            var b = buffer[cursor];
            var bucketIn = b >> 4;

            if (L == _windowSize)
            {
                int bucketOut = ring[head];
                var cOld = hist[bucketOut];
                var cNew = cOld - 1;
                sumTermQ16 += (long)cNew * _log2CountQ16[cNew] - (long)cOld * _log2CountQ16[cOld];
                hist[bucketOut] = cNew;
            }
            else
            {
                L++;
            }

            ring[head] = (byte)bucketIn;
            head++;
            if (head == _windowSize) head = 0;

            {
                var cOld = hist[bucketIn];
                var cNew = cOld + 1;
                sumTermQ16 += (long)cNew * _log2CountQ16[cNew] - (long)cOld * _log2CountQ16[cOld];
                hist[bucketIn] = cNew;
            }

            var hQ16 = CalculateEntropy(L, sumTermQ16);
            if (hQ16 <= _highThreshold)
                return cursor;

            cursor++;
        }

        return upper;
    }

    public string Describe()
    {
        var builder = new DescriptionBuilder("entropy");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("window", _windowSize)
            .AddParameter("low_thresh", _lowThreshold)
            .AddParameter("high_thresh", _highThreshold)
            .Build();
    }

    private long CalculateEntropy(int currentLength, long currentSum)
    {
        var log2L = _log2LengthQ16[currentLength];
        var frac = currentSum / currentLength;
        var h = log2L - frac;
        return h;
    }

    public override string ToString()
    {
        var builder = new DescriptionBuilder("entropy");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .Build();
    }
}