#if LEGACY
using System.Text;
using AnyKit.Pipelines;
using ChunkIt.Common.Abstractions;
using ChunkIt.Common.Extensions;
using ChunkIt.Common.Plotting;
using ScottPlot;
using ScottPlot.TickGenerators;

namespace ChunkIt.Metrics.Deduplication.Plotting;

internal sealed class GenerateDeduplicationPlotPipe : IPlottingPipe
{
    public Task Invoke(PlottingContext context, AsyncPipeline<PlottingContext> next)
    {
        var reportGroups = context
            .Reports
            .GroupBy(
                report => report.SourceFile,
                (sourceFile, reports) => (SourceFile: sourceFile, Reports: reports.ToArray())
            )
            .ToArray();

        var multiplot = new AdaptiveMultiplot(
            rows: reportGroups.Length,
            columns: 2
        );

        foreach (var (sourceFile, reports) in reportGroups)
        {
            var savedRatioPlot = CreateSavedRatioPlot(sourceFile, reports);
            var indexRatioPlot = CreateIndexRatioPlot(sourceFile, reports);

            multiplot.AddPlot(savedRatioPlot);
            multiplot.AddPlot(indexRatioPlot);
        }

        var plotPath = SandboxRuntime.Instance.GetPlotFilePath("deduplication");
        multiplot.Save(plotPath, extraWidth: 500, extraHeight: 500);

        return next(context);
    }

    private static string GenerateSourceFileTitle(SourceFile sourceFile)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"RunId: {SandboxRuntime.Instance.RunId:000}");
        sb.Append($"{sourceFile.Name} ({sourceFile.Size.ToHumanReadableSize()})");

        return sb.ToString();
    }

    private static Plot CreateSavedRatioPlot(SourceFile sourceFile, IReadOnlyList<ChunkingReport> reports)
    {
        var plot = new Plot();

        foreach (var (index, report) in reports.Index())
        {
            var bar = new Bar
            {
                Position = index,
                Value = report.SavedRatio,
                FillColor = PlotColors.ForIndex(index),
                LineColor = PlotColors.ForIndex(index),
                Label = $"{report.SavedBytes.ToHumanReadableSize()} ({report.SavedRatio * 100:F2}%)",
                CenterLabel = true,
            };

            var barPlot = plot.Add.Bar(bar);
            barPlot.ValueLabelStyle = new LabelStyle
            {
                Alignment = Alignment.LowerCenter,
                Rotation = 90,
                Bold = true,
            };
            barPlot.LegendText = report.Partitioner.ToString()!;
        }

        plot.Title(GenerateSourceFileTitle(sourceFile));
        plot.XLabel("Partitioner");
        plot.YLabel("Saved ratio");
        plot.Axes.Margins(bottom: 0);
        plot.ShowLegend(Edge.Left);
        plot.Axes.Bottom.TickGenerator = new EmptyTickGenerator();

        return plot;
    }

    private static Plot CreateIndexRatioPlot(SourceFile _, IReadOnlyList<ChunkingReport> reports)
    {
        var plot = new Plot();

        foreach (var (index, report) in reports.Index())
        {
            var bar = new Bar
            {
                Position = index,
                Value = report.IndexRatio,
                FillColor = PlotColors.ForIndex(index),
                LineColor = PlotColors.ForIndex(index),
                Label = $"{report.IndexBytes.ToHumanReadableSize()} ({report.IndexRatio * 100:F2}%)",
            };

            plot.Add.Bar(bar);
        }

        plot.XLabel("Partitioner");
        plot.YLabel("Index ratio");
        plot.Axes.Margins(bottom: 0);
        plot.Axes.Bottom.TickGenerator = new EmptyTickGenerator();

        return plot;
    }
}
#endif