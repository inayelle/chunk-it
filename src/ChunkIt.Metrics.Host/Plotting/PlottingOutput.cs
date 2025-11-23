namespace ChunkIt.Metrics.Host.Plotting;

internal sealed class PlottingOutput
{
    private const string RootPath = "/storage/ina/workspace/personal/ChunkIt/outputs";

    private readonly string _currentPath;

    public PlottingOutput()
    {
        var rootPath = Environment.GetEnvironmentVariable("CHUNKIT_OUTPUT_ROOT") ?? RootPath;

        var now = DateTimeOffset.Now;

        _currentPath = Path.Combine(
            rootPath,
            $"{now.ToUnixTimeSeconds()}___{now:yyyy-MM-dd___HH-mm-ss}"
        );

        if (!Directory.Exists(_currentPath))
        {
            Directory.CreateDirectory(_currentPath);
        }

        Console.WriteLine($"INF: output path is '{_currentPath}'.");
    }

    public string CreatePathForOutput(params ReadOnlySpan<string> fragments)
    {
        return Path.Combine(
            _currentPath,
            String.Join(".", fragments)
        );
    }
}