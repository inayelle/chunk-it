namespace ChunkIt.Sandbox;

internal static class SourceFilePaths
{
    private const string InputsRoot = "/storage/ina/workspace/personal/ChunkIt/inputs";

    public static IEnumerable<string> Enumerate()
    {
        yield return Path.Combine(InputsRoot, "linux-6.12.44.tar");
        yield return Path.Combine(InputsRoot, "linux-6.16.4.tar");
        // yield return Path.Combine(InputsRoot, "linux-combined.tar");
    }
}