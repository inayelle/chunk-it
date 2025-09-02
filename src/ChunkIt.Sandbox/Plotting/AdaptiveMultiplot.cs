using ScottPlot;
using ScottPlot.MultiplotLayouts;

namespace ChunkIt.Sandbox.Plotting;

internal sealed class AdaptiveMultiplot : Multiplot
{
    public AdaptiveMultiplot(int plotsCount, int columns)
    {
        Layout = new Grid(
            columns: columns,
            rows: (int)Math.Ceiling(plotsCount / (double)columns)
        );

        Subplots.RemoveAt(0);
    }
}