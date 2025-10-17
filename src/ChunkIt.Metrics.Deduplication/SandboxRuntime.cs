namespace ChunkIt.Metrics.Deduplication;

internal sealed class SandboxRuntime : IDisposable
{
    private const string OutputsPath = "/storage/ina/workspace/personal/ChunkIt/outputs";
    private const string RunIdPath = $"{OutputsPath}/.runid";

    private readonly string _plotsPath;

    public static SandboxRuntime Instance { get; } = new SandboxRuntime();

    public int RunId { get; }

    private SandboxRuntime()
    {
        RunId = File.Exists(RunIdPath)
            ? Int32.Parse(File.ReadAllText(RunIdPath)) + 1
            : 0;

        _plotsPath = Path.Combine(OutputsPath, $"{RunId:000}", "plots");
        Directory.CreateDirectory(_plotsPath);

        Console.WriteLine($">>> RUN ID: {RunId:000} >>>");
    }

    public string GetPlotFilePath(string plotName)
    {
        return Path.Combine(_plotsPath, $"{RunId:000}.{plotName}.png");
    }

    public void Dispose()
    {
        File.WriteAllText(RunIdPath, $"{RunId:000}");

        Console.WriteLine($"<<< RUN ID: {RunId:000} <<<");
    }
}