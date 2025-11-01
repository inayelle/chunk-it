using ScottPlot;

namespace ChunkIt.Metrics.Host.Plotting.Abstractions;

public static class PlotColors
{
    private static readonly IReadOnlyList<Color> Colors =
    [
        new Color(221, 44, 44),
        new Color(44, 221, 80),
        new Color(115, 44, 221),
        new Color(221, 150, 44),
        new Color(44, 221, 185),
        new Color(221, 44, 221),
        new Color(185, 221, 44),
        new Color(44, 150, 221),
        new Color(221, 44, 115),
        new Color(80, 221, 44),
        new Color(44, 44, 221),
        new Color(221, 80, 44),
        new Color(44, 221, 115),
        new Color(150, 44, 221),
        new Color(221, 185, 44),
        new Color(44, 221, 221),
        new Color(221, 44, 185),
        new Color(150, 221, 44),
        new Color(44, 115, 221),
        new Color(221, 44, 80),
        new Color(44, 221, 44),
        new Color(80, 44, 221),
        new Color(221, 115, 44),
        new Color(44, 221, 150),
        new Color(185, 44, 221),
        new Color(221, 221, 44),
        new Color(44, 185, 221),
        new Color(221, 44, 150),
        new Color(115, 221, 44),
        new Color(44, 80, 221),
    ];

    public static Color ForIndex(int index)
    {
        if (index >= Colors.Count)
        {
            Console.Error.WriteLine($"No suitable color for index {index}. Falling back to random.");
            return Color.RandomHue();
        }

        return Colors[index];
    }
}