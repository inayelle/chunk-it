using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using ChunkIt.Common.Plotting;

namespace ChunkIt.Metrics.Performance.Exporters;

internal abstract class ScottPlotExporter : IExporter
{
    protected abstract string Type { get; }

    public string Name => $"ScottPlot.{Type}";

    public IEnumerable<string> ExportToFiles(Summary summary, ILogger consoleLogger)
    {
        var plotsDirectoryPath = summary.CreateWorkingDirectory();

        var fileName = $"{Type}.png";
        var filePath = Path.Combine(plotsDirectoryPath, fileName);

        var plot = ExportToPlot(summary);
        plot.Save(filePath, extraWidth: 500);

        yield return filePath;
    }

    protected abstract AdaptiveMultiplot ExportToPlot(Summary summary);

    public void ExportToLog(Summary summary, ILogger logger)
    {
        throw new NotSupportedException();
    }
}

file static class SummaryExtensions
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Now;

    private static string CreateRootDirectory(this Summary summary)
    {
        var rootPath = Path.Combine(summary.ResultsDirectoryPath, "plots");

        if (!Directory.Exists(rootPath))
        {
            Directory.CreateDirectory(rootPath);
        }

        return rootPath;
    }

    public static string CreateWorkingDirectory(this Summary summary)
    {
        var rootPath = summary.CreateRootDirectory();

        var workingPath = Path.Combine(
            rootPath,
            $"{Now.ToUnixTimeMilliseconds()}-{Now:yyyy-MM-dd_HH-mm-ss}"
        );

        if (!Directory.Exists(workingPath))
        {
            Directory.CreateDirectory(workingPath);
        }

        return workingPath;
    }
}