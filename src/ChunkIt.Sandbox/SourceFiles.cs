namespace ChunkIt.Sandbox;

internal static class SourceFiles
{
    private const string InputsRoot = "/storage/ina/workspace/personal/ChunkIt/inputs";

    public static readonly IReadOnlyList<SourceFile> Values =
    [
        Path.Combine(InputsRoot, "linux-6.12.44.tar"),
        Path.Combine(InputsRoot, "linux-6.16.4.tar"),
        "/storage/ina/common/iso/office.iso",
    ];
}