using ChunkIt.Common.Abstractions;

namespace ChunkIt.Metrics.Inputs;

internal static class SourceFilesProvider
{
    public static IEnumerable<SourceFile> Enumerate()
    {
        yield return "/storage/ina/workspace/personal/ChunkIt/inputs/random/random-1GB.bin";
    }
}