using System.Text;
using AnyKit.Pipelines;
using ChunkIt.Common.Extensions;
using ScottPlot;
using ScottPlot.TickGenerators;

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

            plot.Title(GenerateSourceFileTitle(sourceFile));
            plot.XLabel("Partitioner");
            plot.YLabel("Saved ratio");
            plot.Axes.Margins(bottom: 0);
            plot.ShowLegend(Edge.Right);
            plot.Axes.Bottom.TickGenerator = new EmptyTickGenerator();

            multiplot.AddPlot(plot);
        }

        var plotPath = SandboxRuntime.Instance.GetPlotFilePath("saved_ratio");
        multiplot.Save(plotPath, extraWidth: 700);

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