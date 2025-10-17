#if LEGACY
namespace ChunkIt.Metrics.Deduplication.Plotting;

public sealed class PlottingContext
{
    public IReadOnlyList<ChunkingReport> Reports { get; }

    public PlottingContext(IReadOnlyList<ChunkingReport> reports)
    {
        Reports = reports;
    }
}
#endif