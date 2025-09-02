namespace ChunkIt.Sandbox;

internal sealed class FileSystemRoot
{
    private const string OutputsPath = "/storage/ina/workspace/personal/ChunkIt/outputs";
    private const string RunIdPath = $"{OutputsPath}/.runid";

    private readonly string _chunksPath;
    private readonly string _plotsPath;

    public static FileSystemRoot Instance { get; } = new FileSystemRoot();

    public int RunId { get; }

    private FileSystemRoot()
    {
        RunId = File.Exists(RunIdPath)
            ? Int32.Parse(File.ReadAllText(RunIdPath)) + 1
            : 0;
        File.WriteAllText(RunIdPath, RunId.ToString());

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