using ChunkIt.Common.Abstractions;

namespace ChunkIt.Sandbox;

internal static class SourceFiles
{
    public static readonly IReadOnlyList<SourceFile> Values = Enumerate().ToArray();

    private static IEnumerable<SourceFile> Enumerate()
    {
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/debian/debian.tar";
        yield return "/storage/ina/workspace/personal/ChunkIt/inputs/dotnet/dotnet.tar";
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/gcc/gcc.tar";
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/linux/linux.tar";
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/random/random-1GB.bin";
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/random/random-5GB.bin";
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/random/zeros-1GB.bin";
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/random/zeros-5GB.bin";
    }
}