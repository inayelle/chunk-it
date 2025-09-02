namespace ChunkIt.Common.Extensions;

public static class MemoryExtensions
{
    public static (Memory<T> Head, Memory<T> Tail) Split<T>(this Memory<T> memory, int cutPoint)
    {
        if (cutPoint < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cutPoint), "Cut point cannot be less than 0.");
        }

        if (cutPoint > memory.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(cutPoint), "Cut point cannot be further than memory length.");
        }

        var head = memory.Slice(0, cutPoint);
        var tail = memory.Slice(cutPoint);

        return (head, tail);
    }
}