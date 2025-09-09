using System.Text;
using AnyKit.Pipelines;
using ChunkIt.Common.Extensions;
using ScottPlot;

namespace ChunkIt.Sandbox.Plotting;

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
            rows: 1,
            columns: reportGroups.Length
        );

        foreach (var (sourceFile, reports) in reportGroups)
        {
            var plot = new Plot();
            plot.Title(GenerateSourceFileTitle(sourceFile));
            plot.XLabel("Partitioner");
            plot.YLabel("Saved ratio");
            plot.Axes.Margins(bottom: 0);
            plot.ShowLegend(Edge.Bottom);

            foreach (var (index, report) in reports.Index())
            {
                var bar = new Bar
                {
                    Position = index,
                    Value = report.SavedRatio,
                    FillColor = PlotColors.ForIndex(index),
                    LineColor = PlotColors.ForIndex(index),
                    Label = report.SavedBytes.ToHumanReadableSize(),
                };

                var barPlot = plot.Add.Bar(bar);
                barPlot.LegendText = report.Partitioner.ToString()!;
            }

            multiplot.AddPlot(plot);
        }

        var plotPath = SandboxRuntime.Instance.GetPlotFilePath("saved_ratio");
        multiplot.Save(plotPath, extraHeight: 400);

        return next(context);
    }

    private static string GenerateSourceFileTitle(SourceFile sourceFile)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"RunId: {SandboxRuntime.Instance.RunId:000}");
        sb.Append($"{sourceFile.Name} ({sourceFile.Size.ToHumanReadableSize()})");

        return sb.ToString();
    }
}