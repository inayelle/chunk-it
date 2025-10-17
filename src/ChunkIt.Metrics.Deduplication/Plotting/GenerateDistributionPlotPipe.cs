#if LEGACY
using AnyKit.Pipelines;
using ChunkIt.Common.Plotting;
using ChunkIt.Metrics.Deduplication.Extensions;
using ScottPlot;
using ScottPlot.Statistics;

namespace ChunkIt.Metrics.Deduplication.Plotting;

internal sealed class GenerateDistributionPlotPipe : IPlottingPipe
{
    public Task Invoke(
        PlottingContext context,
        AsyncPipeline<PlottingContext> next
    )
    {
        var multiplot = new AdaptiveMultiplot(
            columns: Partitioners.Values.Count,
            rows: SourceFiles.Values.Count
        );

        for (var index = 0; index < context.Reports.Count; index++)
        {
            var report = context.Reports[index];

            var plot = CreatePlot(report, index);

            multiplot.AddPlot(plot);
        }

        var plotPath = SandboxRuntime.Instance.GetPlotFilePath("distribution");
        multiplot.Save(plotPath);

        return next(context);
    }

    private static Plot CreatePlot(ChunkingReport report, int index)
    {
        var plot = new Plot();

        plot.Title(report.SourceFile.ToPlotTitle(report.Partitioner));
        plot.XLabel("Chunk size");
        plot.YLabel("Chunks count");

        var histogram = CreateHistogram(report);

        plot.Add.Histogram(histogram, PlotColors.ForIndex(index));
        plot.Add.Annotation($"Total chunks: {report.Chunks.Count}", alignment: Alignment.UpperLeft);

        return plot;
    }

    private static Histogram CreateHistogram(ChunkingReport report)
    {
        var histogram = Histogram.WithBinSize(
            binSize: 512,
            firstBin: 0,
            lastBin: report.Chunks.Max(chunk => chunk.Length)
        );

        var values = report
            .Chunks
            .Select(chunk => (double)chunk.Length);

        histogram.AddRange(values);

        return histogram;
    }
}
#endif