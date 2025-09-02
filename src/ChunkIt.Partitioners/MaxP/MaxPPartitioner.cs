using ChunkIt.Abstractions;

namespace ChunkIt.Partitioners.MaxP;

public class MaxPPartitioner : IPartitioner
{
    private readonly int _windowSize;

    public int MinimumChunkSize { get; }
    public int AverageChunkSize { get; }
    public int MaximumChunkSize { get; }

    public MaxPPartitioner(
        int minimumChunkSize,
        int averageChunkSize,
        int maximumChunkSize,
        int windowSize
    )
    {
        MinimumChunkSize = minimumChunkSize;
        AverageChunkSize = averageChunkSize;
        MaximumChunkSize = maximumChunkSize;

        _windowSize = windowSize;
    }

    public int FindChunkLength(ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 2 * _windowSize + 1)
        {
            return buffer.Length;
        }

        if (buffer.Length > MaximumChunkSize)
        {
            buffer = buffer.Slice(start: 0, length: MaximumChunkSize);
        }

        var maxPosition = _windowSize;
        var maxValue = buffer[maxPosition];

        for (var i = _windowSize; i < buffer.Length - 1; i++)
        {
            if (buffer[i] >= maxValue)
            {
                maxPosition = i;
                maxValue = buffer[i];
            }
            else if (i == maxPosition + _windowSize)
            {
                var localMaxFound = true;

                for (var j = maxPosition - _windowSize; j < maxPosition; j++)
                {
                    if (buffer[j] <= maxValue)
                    {
                        continue;
                    }

                    maxPosition = i + 1;
                    maxValue = buffer[i + 1];
                    localMaxFound = false;
                    break;
                }

                if (localMaxFound)
                {
                    return maxPosition;
                }
            }
        }

        return buffer.Length;
    }

    public string Describe()
    {
        var builder = new DescriptionBuilder("maxp");

        return builder
            .AddParameter("min", MinimumChunkSize)
            .AddParameter("avg", AverageChunkSize)
            .AddParameter("max", MaximumChunkSize)
            .AddParameter("window", _windowSize)
            .Build();
    }

    public override string ToString()
    {
        return "maxp";
    }
}