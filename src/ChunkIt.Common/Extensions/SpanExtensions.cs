using System.Numerics;

namespace ChunkIt.Common.Extensions;

public static class SpanExtensions
{
    public static bool IsOrderedAscending<T>(this ReadOnlySpan<T> span)
        where T : IComparisonOperators<T, T, bool>
    {
        for (var index = 0; index < span.Length - 1; index++)
        {
            if (span[index] > span[index + 1])
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsOrderedDescending<T>(this ReadOnlySpan<T> span)
        where T : IComparisonOperators<T, T, bool>
    {
        for (var index = 0; index < span.Length - 1; index++)
        {
            if (span[index] < span[index + 1])
            {
                return false;
            }
        }

        return true;
    }
}