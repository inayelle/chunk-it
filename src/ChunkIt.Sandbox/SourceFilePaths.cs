namespace ChunkIt.Sandbox;

internal static class SourceFilePaths
{
    public static IEnumerable<string> Enumerate()
    {
        // yield return Path.Combine("/storage/ina/workspace/personal/ChunkIt/inputs", "1MB-original.json");
        // yield return Path.Combine("/storage/ina/workspace/personal/ChunkIt/inputs", "25MB-original.json");

        // yield return "/home/ina/downloads/linux/linux-6.12.44.tar";
        yield return "/home/ina/downloads/linux/linux-6.16.4.tar";
    }
}