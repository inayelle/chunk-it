using ScottPlot;
using ScottPlot.MultiplotLayouts;

namespace ChunkIt.Sandbox.Plotting;

internal sealed class AdaptiveMultiplot : Multiplot
{
    public int Columns { get; }
    public int Rows { get; }

    public AdaptiveMultiplot(int plotsCount, int columns)
    {
        Columns = columns;
        Rows = (int)Math.Ceiling(plotsCount / (double)columns);

        Layout = new Grid(Rows, Columns);

        Subplots.RemoveAt(0);
    }

    public void Save(string path)
    {
        var plotWidth = 700 * Columns;
        var plotHeight = 500 * Rows;

        this.SavePng(
            Path.Combine(path),
            plotWidth,
            plotHeight
        );
    }
}