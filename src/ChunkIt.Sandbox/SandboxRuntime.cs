namespace ChunkIt.Sandbox;

internal sealed class SandboxRuntime
{
    private const string OutputsPath = "/storage/ina/workspace/personal/ChunkIt/outputs";
    private const string RunIdPath = $"{OutputsPath}/.runid";

    private readonly string _chunksPath;
    private readonly string _plotsPath;

    public static SandboxRuntime Instance { get; } = new SandboxRuntime();

    public int RunId { get; }

    private SandboxRuntime()
    {
        RunId = File.Exists(RunIdPath)
            ? Int32.Parse(File.ReadAllText(RunIdPath)) + 1
            : 0;
        File.WriteAllText(RunIdPath, $"{RunId:000}");

        _chunksPath = Path.Combine(OutputsPath, $"{RunId:000}", "chunks");
        Directory.CreateDirectory(_chunksPath);

        _plotsPath = Path.Combine(OutputsPath, $"{RunId:000}", "plots");
        Directory.CreateDirectory(_plotsPath);
    }

    public string GetChunksFilePath(string partitionerName, string sourceFilePath)
    {
        var sourceFileName = Path.GetFileName(sourceFilePath);

        return Path.Combine(_chunksPath, $"{sourceFileName}.{partitionerName}.chunks");
    }

    public string GetPlotFilePath(string plotName, string sourceFilePath)
    {
        var sourceFileName = Path.GetFileName(sourceFilePath);

        return Path.Combine(_plotsPath, $"{sourceFileName}.{plotName}.png");
    }
}