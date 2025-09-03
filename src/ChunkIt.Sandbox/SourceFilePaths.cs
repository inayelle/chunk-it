namespace ChunkIt.Sandbox;

internal static class SourceFilePaths
{
    private const string InputsRoot = "/storage/ina/workspace/personal/ChunkIt/inputs";

    public static IEnumerable<string> Enumerate()
    {
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/25MB-head.json";
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/25MB-middle.json";
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/25MB-original.json";
        // yield return "/storage/ina/workspace/personal/ChunkIt/inputs/25MB-tail.json";

        yield return Path.Combine(InputsRoot, "linux-6.12.44.tar");
        yield return Path.Combine(InputsRoot, "linux-6.16.4.tar");
        // yield return Path.Combine(InputsRoot, "linux-combined.tar");
    }
}