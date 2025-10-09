using System.Diagnostics;

namespace ChunkIt.Common;

public readonly struct ValueStopwatch
{
    private readonly long _start;

    public TimeSpan Elapsed => Stopwatch.GetElapsedTime(_start);

    private ValueStopwatch(long start)
    {
        _start = start;
    }

    public static ValueStopwatch Start()
    {
        var start = Stopwatch.GetTimestamp();

        return new ValueStopwatch(start);
    }
}