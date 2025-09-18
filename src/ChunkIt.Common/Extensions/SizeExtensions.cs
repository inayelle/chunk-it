namespace ChunkIt.Common.Extensions;

public static class SizeExtensions
{
    private const int Divisor = 1024;

    private static readonly string[] Units =
    [
        "B",
        "KB",
        "MB",
        "GB",
        "TB",
        "EB",
    ];

    public static string ToHumanReadableSize(this long bytesCount)
    {
        var unitIndex = 0;
        var size = (float)bytesCount;

        while (size >= Divisor)
        {
            size /= Divisor;
            unitIndex += 1;
        }

        var unit = Units[unitIndex];

        return $"{size:F2} {unit}";
    }

    public static string ToHumanReadableSize(this int bytesCount)
    {
        var unitIndex = 0;
        var size = (float)bytesCount;

        while (size >= Divisor)
        {
            size /= Divisor;
            unitIndex += 1;
        }

        var unit = Units[unitIndex];

        return $"{size:F2} {unit}";
    }
}