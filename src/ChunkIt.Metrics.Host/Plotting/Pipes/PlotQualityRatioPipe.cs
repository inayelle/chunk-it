using AnyKit.Pipelines;
using ChunkIt.Common.Abstractions;
using ChunkIt.Metrics.Host.Plotting.Abstractions;
using ScottPlot;
using ScottPlot.TickGenerators;

namespace ChunkIt.Metrics.Host.Plotting.Pipes;

internal sealed class PlotQualityPipe : IPlottingPipe
{
    public Task Invoke(PlottingContext context, AsyncPipeline<PlottingContext> next)
    {
        foreach (var (sourceFile, reports) in context.Reports.GroupBySourceFile())
        {
            var multiplot = AdaptiveMultiplot.WithRows(
                rows: 1,
                totalCount: 1
            );

            var plot = new QualityPlot(sourceFile, reports);

            multiplot.AddPlot(plot);

            var multiplotPath = context.Output.CreatePathForOutput(
                sourceFile.Name,
                "quality",
                "png"
            );

            multiplot.Save(multiplotPath, width: 800, height: 600);
        }

        return next(context);
    }
}

file sealed class QualityPlot : Plot
{
    public QualityPlot(SourceFile sourceFile, IReadOnlyCollection<ChunkingReport> reports)
    {
        AddBars(reports);
        AddLegend(reports);

        Title(sourceFile.ToString());
        XLabel("Метод фрагментації даних");
        YLabel("Геометричний коефіцієнт якості");

        Axes.Margins(bottom: 0);
        Axes.Left.TickGenerator = new NumericAutomatic();
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
            var name = report.Input.Partitioner.Name;
            var value = Math.Round(report.Deduplication.QualityRatio, 2);

            var color = PlotColors.ForIndex(index);

            legend.ManualItems.Add(new LegendItem
                {
                    LabelText = $"{name} ({value})",
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
            Value = report.Deduplication.QualityRatio,
            FillColor = color,
            LineColor = color,
        };
    }
}