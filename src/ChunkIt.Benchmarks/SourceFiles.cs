using ChunkIt.Common.Abstractions;

namespace ChunkIt.Benchmarks;

public static class SourceFiles
{
    public static IEnumerable<SourceFile> Enumerate()
    {
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/dotnet/dotnet.tar";
        yield return "/storage/ina/workspace/personal/ChunkIt/inputs/random/random-1GB.bin";
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/random/random-5GB.bin";
    }
}