using System.Text;
using AnyKit.Pipelines;
using ChunkIt.Common.Abstractions;
using ChunkIt.Common.Extensions;
using ScottPlot;
using ScottPlot.TickGenerators;

namespace ChunkIt.Sandbox.Plotting;

internal sealed class GenerateQualityPlotPipe : IPlottingPipe
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
            columns: 1
        );

        foreach (var (sourceFile, reports) in reportGroups)
        {
            var qualityPlot = CreateQualityPlot(sourceFile, reports);

            multiplot.AddPlot(qualityPlot);
        }

        var plotPath = SandboxRuntime.Instance.GetPlotFilePath("quality");
        multiplot.Save(plotPath, extraWidth: 500, extraHeight: 500);

        return next(context);
    }

    private static Plot CreateQualityPlot(
        SourceFile sourceFile,
        IReadOnlyList<ChunkingReport> reports
    )
    {
        var plot = new Plot();

        foreach (var (index, report) in reports.Index())
        {
            var bar = new Bar
            {
                Position = index,
                Value = report.QualityRatio,
                FillColor = PlotColors.ForIndex(index),
                LineColor = PlotColors.ForIndex(index),
                Label = $"{report.QualityRatio * 100:F2}%",
            };

            var barPlot = plot.Add.Bar(bar);
            barPlot.LegendText = report.Partitioner.ToString()!;
        }

        plot.Title(GenerateSourceFileTitle(sourceFile));
        plot.XLabel("Partitioner");
        plot.YLabel("Quality ratio");
        plot.Axes.Margins(bottom: 0);
        plot.ShowLegend(Edge.Left);
        plot.Axes.Bottom.TickGenerator = new EmptyTickGenerator();

        return plot;
    }

    private static string GenerateSourceFileTitle(SourceFile sourceFile)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"RunId: {SandboxRuntime.Instance.RunId:000}");
        sb.Append($"{sourceFile.Name} ({sourceFile.Size.ToHumanReadableSize()})");

        return sb.ToString();
    }
}