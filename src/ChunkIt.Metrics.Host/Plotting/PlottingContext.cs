using ChunkIt.Common.Abstractions;

namespace ChunkIt.Metrics.Host.Plotting;

internal sealed class PlottingContext
{
    public IReadOnlyList<ChunkingReport> Reports { get; }

    public PlottingOutput Output { get; }

    public int SourceFilesCount { get; }
    public int PartitionersCount { get; }

    public PlottingContext(IReadOnlyList<ChunkingReport> reports)
    {
        Reports = reports;
        Output = new PlottingOutput();

        SourceFilesCount = Reports
            .Select(report => report.Input.SourceFile)
            .Distinct()
            .Count();

        PartitionersCount = Reports
            .Select(report => report.Input.Partitioner)
            .Distinct()
            .Count();
    }
}

internal static class Extensions
{
    public static IEnumerable<(SourceFile SourceFile, ChunkingReport[] Reports)> GroupBySourceFile(
        this IEnumerable<ChunkingReport> reports
    )
    {
        return reports.GroupBy(
            static report => report.Input.SourceFile,
            static (sourceFile, reports) => (sourceFile, reports.Order().ToArray())
        );
    }
}