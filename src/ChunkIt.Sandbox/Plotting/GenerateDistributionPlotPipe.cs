using AnyKit.Pipelines;
using ChunkIt.Common.Extensions;
using ScottPlot;
using ScottPlot.Statistics;

namespace ChunkIt.Sandbox.Plotting;

internal sealed class GenerateDistributionPlotPipe : IPlottingPipe
{
    private static readonly Color[] Colors =
    [
        ScottPlot.Colors.Red,
        ScottPlot.Colors.Green,
        ScottPlot.Colors.Blue,
        ScottPlot.Colors.Yellow,
        ScottPlot.Colors.Pink,
        ScottPlot.Colors.Brown,
        ScottPlot.Colors.Purple,
        ScottPlot.Colors.Beige,
        ScottPlot.Colors.Orange,
    ];

    public Task Invoke(
        PlottingContext context,
        AsyncPipeline<PlottingContext> next
    )
    {
        var multiplot = new AdaptiveMultiplot(context.Reports.Count);

        for (var index = 0; index < context.Reports.Count; index++)
        {
            var report = context.Reports[index];
            var plot = new Plot();

            plot.Title($"{report.Partitioner.Describe()}\n(RunId: {FileSystemRoot.Instance.RunId:000})");
            plot.XLabel("Chunk size");
            plot.YLabel("Chunks count");

            var histogram = CreateHistogram(
                report,
                report.Partitioner.MinimumChunkSize,
                report.Partitioner.MaximumChunkSize
            );

            plot.Add.Histogram(histogram, color: Colors[index]);
            plot.Add.Annotation(
                report.FileName,
                Alignment.UpperRight
            );
            plot.Add.Annotation(
                $"Saved ratio: {report.SavedRatio:F2}%; " +
                $"before: {report.OriginalFileSize.ToHumanReadableSize()}; " +
                $"after: {report.CompressedFileSize.ToHumanReadableSize()}",
                Alignment.MiddleRight
            );
            plot.Add.Annotation(
                $"Total chunks: {report.TotalChunks}",
                Alignment.LowerRight
            );

            multiplot.AddPlot(plot);
        }

        var plotPath = FileSystemRoot.Instance.GetPlotFilePath(
            "distribution",
            context.SourceFilePath
        );

        multiplot.SavePng(
            Path.Combine(plotPath),
            width: 1600,
            height: 900
        );

        return next(context);
    }

    private static Histogram CreateHistogram(
        ChunkingReport report,
        int minimumChunkSize,
        int maximumChunkSize
    )
    {
        var histogram = Histogram.WithBinSize(
            binSize: 256,
            firstBin: minimumChunkSize,
            lastBin: maximumChunkSize
        );

        var values = report
            .Chunks
            .Select(chunk => (double)chunk.Length);

        histogram.AddRange(values);

        return histogram;
    }
}