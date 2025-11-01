using AnyKit.Pipelines;
using ChunkIt.Metrics.Deduplication;
using ChunkIt.Metrics.Host.Plotting.Abstractions;
using ScottPlot;
using ScottPlot.Statistics;

namespace ChunkIt.Metrics.Host.Plotting.Pipes;

internal sealed class PlotChunkDistributionPipe : IPlottingPipe
{
    public Task Invoke(
        PlottingContext context,
        AsyncPipeline<PlottingContext> next
    )
    {
        foreach (var (sourceFile, reports) in context.Reports.GroupBySourceFile())
        {
            var multiplot = AdaptiveMultiplot.WithColumns(
                columns: 2,
                totalCount: context.PartitionersCount
            );

            foreach (var report in reports)
            {
                var plot = new DistributionPlot(report);

                multiplot.AddPlot(plot);
            }

            var multiplotPath = context.Output.CreatePathForOutput(
                sourceFile.Name,
                "distribution",
                "png"
            );

            multiplot.Save(multiplotPath, extraWidth: 250);
        }

        return next(context);
    }
}

file sealed class DistributionPlot : Plot
{
    public DistributionPlot(ChunkingReport report)
    {
        var color = PlotColors.ForIndex(report.Input.Index);

        XLabel("Chunk size");
        YLabel("Chunks count");

        var histogram = Add.Histogram(
            CreateHistogram(report.Deduplication),
            color
        );

        var annotation = Add.Annotation(
            $"Total chunks: {report.Deduplication.Chunks.Count}",
            alignment: Alignment.UpperLeft
        );

        var legend = ShowLegend(Edge.Left).Legend;

        legend.ManualItems.Add(new LegendItem
            {
                LabelText = report.Input.Partitioner.ToString()!,
                MarkerStyle = new MarkerStyle
                {
                    FillColor = color,
                    LineColor = color,
                    Size = 24,
                    Shape = MarkerShape.HorizontalBar,
                },
            }
        );
    }

    private static Histogram CreateHistogram(DeduplicationReport report)
    {
        var chunkSizes = report
            .Chunks
            .Select(chunk => (double)chunk.Length)
            .ToArray();

        var histogram = Histogram.WithBinSize(
            binSize: 512,
            firstBin: 0,
            lastBin: chunkSizes.Max()
        );

        histogram.AddRange(chunkSizes);

        return histogram;
    }
}