using ChunkIt.Common.Abstractions;

namespace ChunkIt.Metrics.Inputs;

internal static class SourceFilesProvider
{
    private const string Root = "/storage/ina/workspace/personal/ChunkIt/inputs";

    private static readonly string Linux = Path.Combine(Root, "linux/linux.tar");
    private static readonly string Dotnet = Path.Combine(Root, "dotnet/dotnet.tar");
    private static readonly string Gcc = Path.Combine(Root, "gcc/gcc.tar");
    private static readonly string Random = Path.Combine(Root, "random/random.bin");

    public static IEnumerable<SourceFile> Enumerate()
    {
        yield return Linux;
        yield return Dotnet;
        yield return Gcc;
        yield return Random;
    }
}