using ScottPlot;

namespace ChunkIt.Sandbox.Plotting;

internal static class PlotColors
{
    private static readonly IReadOnlyList<Color> Colors =
    [
        ScottPlot.Colors.Red,
        ScottPlot.Colors.Green,
        ScottPlot.Colors.Blue,
        ScottPlot.Colors.Yellow,
        ScottPlot.Colors.Pink,
        ScottPlot.Colors.Brown,
        ScottPlot.Colors.Purple,
        ScottPlot.Colors.Beige,
        ScottPlot.Colors.Orange,
        ScottPlot.Colors.Turquoise,
        ScottPlot.Colors.Silver,
        ScottPlot.Colors.Orchid,
        ScottPlot.Colors.Crimson,
        ScottPlot.Colors.Cyan,
        ScottPlot.Colors.Gold,
        ScottPlot.Colors.AliceBlue,
        ScottPlot.Colors.IndianRed,
        ScottPlot.Colors.Navy,
        ScottPlot.Colors.FireBrick,
        ScottPlot.Colors.Magenta,
        ScottPlot.Colors.Teal,
        ScottPlot.Colors.SandyBrown,
        ScottPlot.Colors.Olive,
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