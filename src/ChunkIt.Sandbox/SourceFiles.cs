namespace ChunkIt.Sandbox;

internal static class SourceFiles
{
    private const string InputsRoot = "/storage/ina/workspace/personal/ChunkIt/inputs";

    public static readonly IReadOnlyList<SourceFile> Values = Enumerate().ToArray();

    private static IEnumerable<SourceFile> Enumerate()
    {
        yield return "/storage/ina/workspace/personal/ChunkIt/inputs/dotnet/dotnet.tar";
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/linux/linux.tar";
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/gcc/gcc.tar";
    }
}