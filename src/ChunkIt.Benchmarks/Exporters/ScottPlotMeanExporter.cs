using System.Text;
using BenchmarkDotNet.Helpers;
using BenchmarkDotNet.Reports;
using ChunkIt.Benchmarks.Extensions;
using ChunkIt.Common.Abstractions;
using ChunkIt.Common.Extensions;
using ChunkIt.Common.Plotting;
using Perfolizer.Horology;
using ScottPlot;
using ScottPlot.TickGenerators;

namespace ChunkIt.Benchmarks.Exporters;

internal sealed class ScottPlotMeanExporter : ScottPlotExporter
{
    protected override string Type => "mean";

    protected override AdaptiveMultiplot ExportToPlot(Summary summary)
    {
        var groups = summary.GetBenchmarkGroups();

        var multiplot = new AdaptiveMultiplot(
            rows: groups.Length,
            columns: 1
        );

        foreach (var group in groups)
        {
            var plot = new Plot();

            foreach (var (index, result) in group.Results.Index())
            {
                var bar = new Bar
                {
                    Position = index,
                    Value = result.Mean.ToUnit(TimeUnit.Millisecond),
                    FillColor = PlotColors.ForIndex(index),
                    LineColor = PlotColors.ForIndex(index),
                    Label = result.Mean.ToDefaultString(),
                    CenterLabel = true,
                };

                var barPlot = plot.Add.Bar(bar);
                barPlot.ValueLabelStyle = new LabelStyle
                {
                    Alignment = Alignment.MiddleCenter,
                    Rotation = 0,
                    Bold = true,
                };
                barPlot.LegendText = result.Partitioner.ToString()!;
            }

            plot.Title(GeneratePlotTitle(group.SourceFile));
            plot.XLabel("Partitioner");
            plot.YLabel("Mean time, ms");
            plot.Axes.Margins(bottom: 0);
            plot.ShowLegend(Edge.Left);
            plot.Axes.Bottom.TickGenerator = new EmptyTickGenerator();

            multiplot.AddPlot(plot);
        }

        return multiplot;
    }

    private static string GeneratePlotTitle(SourceFile sourceFile)
    {
        var sb = new StringBuilder();

        sb.Append($"{sourceFile.Name} ({sourceFile.Size.ToHumanReadableSize()})");

        return sb.ToString();
    }
}