namespace ChunkIt.Sandbox.Plotting;

internal sealed class PlottingContext
{
    public string SourceFilePath { get; }

    public IReadOnlyList<ChunkingReport> Reports { get; }

    public PlottingContext(
        string sourceFilePath,
        IReadOnlyList<ChunkingReport> reports
    )
    {
        SourceFilePath = sourceFilePath;
        Reports = reports;
    }
}