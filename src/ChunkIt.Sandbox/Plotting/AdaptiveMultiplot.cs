using ScottPlot;
using ScottPlot.MultiplotLayouts;

namespace ChunkIt.Sandbox.Plotting;

internal sealed class AdaptiveMultiplot : Multiplot
{
    public int Columns { get; }
    public int Rows { get; }

    public AdaptiveMultiplot(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;

        Layout = new Grid(Rows, Columns);
        Subplots.RemoveAt(0);
    }

    public static AdaptiveMultiplot FromColumns(int columns, int totalCount)
    {
        var rows = (int)Math.Ceiling(totalCount / (double)columns);

        return new AdaptiveMultiplot(rows, columns);
    }

    public static AdaptiveMultiplot FromRows(int rows, int totalCount)
    {
        var columns = (int)Math.Ceiling(totalCount / (double)rows);

        return new AdaptiveMultiplot(rows, columns);
    }

    public void Save(string path, int extraWidth = 0, int extraHeight = 0)
    {
        var plotWidth = 700 * Columns + extraWidth;
        var plotHeight = 500 * Rows + extraHeight;

        this.SavePng(
            Path.Combine(path),
            plotWidth,
            plotHeight
        );
    }
}