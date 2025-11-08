using AnyKit.Pipelines;
using ChunkIt.Common.Abstractions;
using ChunkIt.Metrics.Host.Plotting.Abstractions;
using ScottPlot;
using ScottPlot.TickGenerators;

namespace ChunkIt.Metrics.Host.Plotting.Pipes;

internal sealed class PlotDeduplicationThroughputPipe : IPlottingPipe
{
    public Task Invoke(PlottingContext context, AsyncPipeline<PlottingContext> next)
    {
        var multiplot = AdaptiveMultiplot.WithColumns(
            columns: 2,
            totalCount: context.SourceFilesCount
        );

        foreach (var (sourceFile, reports) in context.Reports.GroupBySourceFile())
        {
            var plot = new DeduplicationThroughputPlot(sourceFile, reports);

            multiplot.AddPlot(plot);
        }

        var multiplotPath = context.Output.CreatePathForOutput(
            "deduplication_throughput",
            "png"
        );

        multiplot.Save(multiplotPath, width: 1600, height: 600);

        return next(context);
    }
}

file sealed class DeduplicationThroughputPlot : Plot
{
    public DeduplicationThroughputPlot(SourceFile sourceFile, IReadOnlyCollection<ChunkingReport> reports)
    {
        AddBars(reports);
        AddLegend(reports);

        Title(sourceFile.ToString());
        YLabel("Пропускна здатність дедублікації");

        Axes.Margins(bottom: 0);
        Axes.Left.TickGenerator = new NumericWithUnitTickGenerator("Gb/s");
        Axes.Bottom.TickGenerator = new EmptyTickGenerator();
    }

    private void AddBars(IReadOnlyCollection<ChunkingReport> reports)
    {
        var bars = reports.Select(CreateBar).ToArray();

        Add.Bars(bars);
    }

    private void AddLegend(IReadOnlyCollection<ChunkingReport> reports)
    {
        var legend = ShowLegend(Edge.Left).Legend;

        foreach (var (index, report) in reports.Index())
        {
            var color = PlotColors.ForIndex(index);
            var name = report.Input.Partitioner.Name;
            var value = Math.Round(report.SavedBytesThroughput.GigabitsPerSecond, 2);

            legend.ManualItems.Add(new LegendItem
                {
                    LabelText = $"{name} ({value} Gb/s)",
                    MarkerStyle = new MarkerStyle
                    {
                        FillColor = color,
                        LineColor = color,
                        Size = 16,
                        Shape = MarkerShape.FilledSquare,
                    },
                }
            );
        }
    }

    private static Bar CreateBar(ChunkingReport report, int index)
    {
        var color = PlotColors.ForIndex(index);

        return new Bar
        {
            Position = report.Input.Index,
            Value = (double)report.SavedBytesThroughput.GigabitsPerSecond,
            FillColor = color,
            LineColor = color,
        };
    }
}