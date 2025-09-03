using System.Text;
using AnyKit.Pipelines;
using ChunkIt.Common.Extensions;
using ScottPlot;
using ScottPlot.Statistics;

namespace ChunkIt.Sandbox.Plotting;

internal sealed class GenerateDistributionPlotPipe : IPlottingPipe
{
    public Task Invoke(
        PlottingContext context,
        AsyncPipeline<PlottingContext> next
    )
    {
        var multiplot = new AdaptiveMultiplot(
            columns: Partitioners.Enumerate().Count(),
            rows: SourceFilePaths.Enumerate().Count()
        );

        for (var index = 0; index < context.Reports.Count; index++)
        {
            var report = context.Reports[index];

            var plot = CreatePlot(report, index);

            multiplot.AddPlot(plot);
        }

        var plotPath = SandboxRuntime.Instance.GetPlotFilePath(
            "distribution",
            $"{SandboxRuntime.Instance.RunId:000}"
        );

        multiplot.Save(Path.Combine(plotPath));

        return next(context);
    }

    private static Plot CreatePlot(ChunkingReport report, int index)
    {
        var plot = new Plot();

        plot.Title(GenerateReportTitle(report));
        plot.XLabel("Chunk size");
        plot.YLabel("Chunks count");

        var histogram = CreateHistogram(report);
        plot.Add.Histogram(histogram, PlotColors.ForIndex(index));

        plot.Add.Annotation(
            $"Saved ratio: {report.SavedRatio:F2}%\n" +
            $"Saved size: {report.SavedBytes.ToHumanReadableSize()}",
            Alignment.UpperLeft
        );
        plot.Add.Annotation(
            $"Total chunks: {report.TotalChunks}",
            Alignment.MiddleLeft
        );

        return plot;
    }

    private static Histogram CreateHistogram(ChunkingReport report)
    {
        var histogram = Histogram.WithBinSize(
            binSize: 256,
            firstBin: report.Partitioner.MinimumChunkSize,
            lastBin: report.Partitioner.MaximumChunkSize
        );

        var values = report
            .Chunks
            .Select(chunk => (double)chunk.Length);

        histogram.AddRange(values);

        return histogram;
    }

    private static string GenerateReportTitle(ChunkingReport report)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"RunId: {SandboxRuntime.Instance.RunId:000}");
        sb.AppendLine($"{report.FileName} ({report.OriginalFileSize.ToHumanReadableSize()})");
        sb.Append(report.Partitioner.Describe());

        return sb.ToString();
    }
}

file static class PlotColors
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