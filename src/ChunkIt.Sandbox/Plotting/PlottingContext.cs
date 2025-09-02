namespace ChunkIt.Sandbox.Plotting;

internal sealed class PlottingContext
{
    public IReadOnlyList<ChunkingReport> Reports { get; }

    public PlottingContext(IReadOnlyList<ChunkingReport> reports)
    {
        Reports = reports;
    }
}