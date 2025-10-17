#if LEGACY
using AnyKit.Pipelines;
using ChunkIt.Common.Abstractions;
using ChunkIt.Common.Plotting;
using ChunkIt.Metrics.Deduplication.Extensions;
using ScottPlot;
using ScottPlot.TickGenerators;

namespace ChunkIt.Metrics.Deduplication.Plotting;

internal sealed class GenerateVariancePlotPipe : IPlottingPipe
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
            var qualityPlot = CreateVariancePlot(sourceFile, reports);

            multiplot.AddPlot(qualityPlot);
        }

        var plotPath = SandboxRuntime.Instance.GetPlotFilePath("variance");
        multiplot.Save(plotPath, extraWidth: 500, extraHeight: 500);

        return next(context);
    }

    private static Plot CreateVariancePlot(
        SourceFile sourceFile,
        IReadOnlyList<ChunkingReport> reports
    )
    {
        var plot = new Plot();

        foreach (var (index, report) in reports.Index())
        {
            var bar = new VarianceBar(report, index);

            var barPlot = plot.Add.Bar(bar);
            barPlot.LegendText = report.Partitioner.ToString()!;
        }

        ConfigurePlot(plot, sourceFile.ToPlotTitle());

        return plot;
    }

    private static void ConfigurePlot(Plot plot, string title)
    {
        plot.Title(title);
        plot.XLabel("Partitioner");
        plot.YLabel("Variance ratio");
        plot.ShowLegend(Edge.Left);

        plot.Axes.Margins(bottom: 0);

        plot.Axes.Bottom.TickGenerator = new EmptyTickGenerator();
    }
}

file sealed class VarianceBar : Bar
{
    public VarianceBar(ChunkingReport report, int index)
    {
        Position = index;
        Value = report.VarianceRatio;
        Label = $"{report.VarianceRatio:F2}";

        FillColor = LineColor = PlotColors.ForIndex(index);
    }
}
#endif