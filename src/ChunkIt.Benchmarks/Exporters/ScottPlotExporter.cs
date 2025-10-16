using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using ChunkIt.Common.Plotting;

namespace ChunkIt.Benchmarks.Exporters;

internal abstract class ScottPlotExporter : IExporter
{
    protected abstract string Type { get; }

    public string Name => $"ScottPlot.{Type}";

    public IEnumerable<string> ExportToFiles(Summary summary, ILogger consoleLogger)
    {
        var plotDirectoryPath = Path.Combine(summary.ResultsDirectoryPath, "plots");

        if (!Directory.Exists(plotDirectoryPath))
        {
            Directory.CreateDirectory(plotDirectoryPath);
        }

        var fileName = $"plot.{Type}.png";
        var filePath = Path.Combine(plotDirectoryPath, fileName);

        var plot = ExportToPlot(summary);
        plot.Save(filePath);

        yield return filePath;
    }

    protected abstract AdaptiveMultiplot ExportToPlot(Summary summary);

    public void ExportToLog(Summary summary, ILogger logger)
    {
        throw new NotSupportedException();
    }
}