using BenchmarkDotNet.Reports;
using ChunkIt.Metrics.Performance.Extensions;
using ChunkIt.Common.Plotting;
using ScottPlot;
using ScottPlot.TickGenerators;
using UnitsNet.Units;

namespace ChunkIt.Metrics.Performance.Exporters;

internal sealed class ScottPlotThroughputExporter : ScottPlotExporter
{
    protected override string Type => "throughput";

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
                var bar = new ThroughputBar(result, index);

                var barPlot = plot.Add.Bar(bar);
                barPlot.LegendText = result.Partitioner.ToString()!;
            }

            ConfigurePlot(plot, group.SourceFile.ToPlotTitle());

            multiplot.AddPlot(plot);
        }

        return multiplot;
    }

    private static void ConfigurePlot(Plot plot, string title)
    {
        plot.Title(title);
        plot.XLabel("Partitioner");
        plot.YLabel("Throughput");
        plot.ShowLegend(Edge.Left);

        plot.Axes.Margins(bottom: 0);

        plot.Axes.Bottom.TickGenerator = new EmptyTickGenerator();
        plot.Axes.Left.TickGenerator = new NumericWithUnitTickGenerator("Gbit/s");
    }
}

file sealed class ThroughputBar : Bar
{
    public ThroughputBar(ChunkingBenchmarkResult result, int index)
    {
        var throughput = result.Throughput.ToUnit(BitRateUnit.GigabitPerSecond);

        Position = index;
        Value = (double)throughput.GigabitsPerSecond;
        Label = throughput.ToString();

        FillColor = LineColor = PlotColors.ForIndex(index);
    }
}