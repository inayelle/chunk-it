using ChunkIt.Common.Abstractions;
using ChunkIt.Common.Extensions;

namespace ChunkIt.Metrics.Performance.Extensions;

internal static class SourceFileExtensions
{
    public static string ToPlotTitle(this SourceFile sourceFile)
    {
        return $"{sourceFile.Name} ({sourceFile.Size.ToHumanReadableSize()})";
    }
}