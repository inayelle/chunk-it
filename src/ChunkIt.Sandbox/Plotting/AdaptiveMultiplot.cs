using ScottPlot;
using ScottPlot.MultiplotLayouts;

namespace ChunkIt.Sandbox.Plotting;

internal sealed class AdaptiveMultiplot : Multiplot
{
    public AdaptiveMultiplot(int plotsCount)
    {
        Layout = new Grid(
            columns: 2,
            rows: (int)Math.Ceiling(plotsCount / 2.0)
        );

        Subplots.RemoveAt(0);
    }
}