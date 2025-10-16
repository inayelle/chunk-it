using System.Text;
using AnyKit.Pipelines;
using ChunkIt.Common.Extensions;
using ChunkIt.Common.Plotting;
using ScottPlot;
using ScottPlot.Statistics;

namespace ChunkIt.Sandbox.Plotting;

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

        plot.Title(GenerateReportTitle(report));
        plot.XLabel("Chunk size");
        plot.YLabel("Chunks count");

        var histogram = CreateHistogram(report);

        plot.Add.Histogram(histogram, PlotColors.ForIndex(index));
        plot.Add.Annotation($"Total chunks: {report.Chunks.Count}", alignment: Alignment.UpperLeft);
        plot.Add.Annotation($"Quality ratio: {report.QualityRatio * 100:F2}%", alignment: Alignment.LowerLeft);

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

    private static string GenerateReportTitle(ChunkingReport report)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"RunId: {SandboxRuntime.Instance.RunId:000}");
        sb.AppendLine($"{report.SourceFile.Name} ({report.SourceFile.Size.ToHumanReadableSize()})");
        sb.Append(report.Partitioner);

        return sb.ToString();
    }
}