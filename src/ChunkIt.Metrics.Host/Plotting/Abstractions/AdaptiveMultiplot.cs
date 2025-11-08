using ScottPlot;
using ScottPlot.MultiplotLayouts;

namespace ChunkIt.Metrics.Host.Plotting.Abstractions;

public sealed class AdaptiveMultiplot : Multiplot
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

    public void Save(
        string path,
        int? width = null,
        int? height = null,
        int? extraWidth = null,
        int? extraHeight = null
    )
    {
        width ??= 700 * Columns + (extraWidth ?? 0);
        height ??= 500 * Rows + (extraHeight ?? 0);

        this.SavePng(path, width.Value, height.Value);
    }

    public static AdaptiveMultiplot WithRows(int rows, int totalCount)
    {
        var columns = (int)Math.Ceiling(totalCount / (double)rows);

        return new AdaptiveMultiplot(rows, columns);
    }

    public static AdaptiveMultiplot WithColumns(int columns, int totalCount)
    {
        var rows = (int)Math.Ceiling(totalCount / (double)columns);

        return new AdaptiveMultiplot(rows, columns);
    }
}