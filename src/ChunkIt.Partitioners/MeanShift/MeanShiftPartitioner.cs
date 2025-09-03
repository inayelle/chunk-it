using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Partitioners.MeanShift;

public sealed class MeanShiftPartitioner : IPartitioner
{
    private const double Epsilon = 1e-9;

    private readonly int _windowSize;
    private readonly double _kPre, _kPost;
    private readonly int _threshold;

    public int MinimumChunkSize { get; }
    public int AverageChunkSize { get; }
    public int MaximumChunkSize { get; }

    public MeanShiftPartitioner(
        int minimumChunkSize,
        int averageChunkSize,
        int maximumChunkSize,
        int window = 32,
        double kPre = 2.0,
        double kPost = 1.4,
        int threshold = 6
    )
    {
        MinimumChunkSize = minimumChunkSize;
        AverageChunkSize = averageChunkSize;
        MaximumChunkSize = maximumChunkSize;
        _windowSize = window;
        _kPre = kPre;
        _kPost = kPost;
        _threshold = threshold;
    }

    public int FindChunkLength(ReadOnlySpan<byte> buffer)
    {
        var len = buffer.Length;
        if (len <= MinimumChunkSize) return len;

        var upper = Math.Min(len, MaximumChunkSize);
        var mid = Math.Min(upper, AverageChunkSize);

        // Стартуємо з першого дозволеного байта
        var cursor = MinimumChunkSize;

        // Кільце і статистики для вікна попередніх байтів
        var L = Math.Min(_windowSize, cursor);
        Span<byte> ring = stackalloc byte[_windowSize];
        long sum = 0, sumSq = 0;
        var head = 0;

        // Заповнюємо попереднє вікно [cursor - L .. cursor - 1]
        var start = cursor - L;
        for (var i = start; i < cursor; i++)
        {
            var v = buffer[i];
            ring[head++] = v;
            sum += v;
            sumSq += (long)v * v;
        }

        if (head == _windowSize) head = 0;

        var badCount = 0;

        // ---- Фаза до avg (строгіший поріг) ----
        while (cursor < mid)
        {
            // mean/var на основі попереднього вікна (L >= 1)
            var mu = (double)sum / L;
            double var;
            if (L > 1)
            {
                var = (sumSq - (double)sum * sum / L) / (L - 1);
                if (var < Epsilon) var = Epsilon;
            }
            else
            {
                var = Epsilon;
            }

            double x = buffer[cursor];
            var d = x - mu;
            var d2 = d * d;
            var thr2 = _kPre * _kPre * var;

            if (d2 > thr2)
            {
                badCount++;
                if (badCount >= _threshold)
                {
                    var cut = cursor - badCount;
                    if (cut < MinimumChunkSize) cut = MinimumChunkSize;
                    return cut; // межа на останньому "гарному" байті
                }
            }
            else
            {
                badCount = 0;
            }

            // Зсунути вікно: додаємо поточний, викидаємо найстаріший (якщо L == W)
            var inByte = buffer[cursor];
            if (L == _windowSize)
            {
                var outByte = ring[head];
                sum -= outByte;
                sumSq -= (long)outByte * outByte;
            }
            else
            {
                L++;
            }

            ring[head] = inByte;
            head++;
            if (head == _windowSize) head = 0;

            sum += inByte;
            sumSq += (long)inByte * inByte;

            cursor++;
        }

        // ---- Фаза після avg (м’якший поріг) ----
        while (cursor < upper)
        {
            var mu = (double)sum / L;
            double var;
            if (L > 1)
            {
                var = (sumSq - (double)sum * sum / L) / (L - 1);
                if (var < Epsilon) var = Epsilon;
            }
            else
            {
                var = Epsilon;
            }

            double x = buffer[cursor];
            var d = x - mu;
            var d2 = d * d;
            var thr2 = _kPost * _kPost * var;

            if (d2 > thr2)
            {
                badCount++;
                if (badCount >= _threshold)
                {
                    var cut = cursor - badCount;
                    if (cut < MinimumChunkSize) cut = MinimumChunkSize;
                    return cut;
                }
            }
            else
            {
                badCount = 0;
            }

            var inByte = buffer[cursor];
            if (L == _windowSize)
            {
                var outByte = ring[head];
                sum -= outByte;
                sumSq -= (long)outByte * outByte;
            }
            else
            {
                L++;
            }

            ring[head] = inByte;
            head++;
            if (head == _windowSize) head = 0;

            sum += inByte;
            sumSq += (long)inByte * inByte;

            cursor++;
        }

        // Нічого не спрацювало — форс на upper
        return upper;
    }

    public string Describe()
    {
        var builder = new DescriptionBuilder("mean-shift");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("window", _windowSize)
            .AddParameter("kPre", _kPre)
            .AddParameter("kPost", _kPost)
            .Build();
    }

    public override string ToString()
    {
        var builder = new DescriptionBuilder("mean-shift");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .Build();
    }
}