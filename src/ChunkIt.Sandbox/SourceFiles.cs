namespace ChunkIt.Sandbox;

internal static class SourceFiles
{
    private const string InputsRoot = "/storage/ina/workspace/personal/ChunkIt/inputs";

    public static readonly IReadOnlyList<SourceFile> Values = Enumerate().ToArray();

    private static IEnumerable<SourceFile> Enumerate()
    {
        // yield return Path.Combine(InputsRoot, "linux-6.16.4.tar");

        yield return "/home/ina/downloads/gcc/gcc.tar";
        // yield return "/storage/ina/common/iso/office.iso";
    }
}