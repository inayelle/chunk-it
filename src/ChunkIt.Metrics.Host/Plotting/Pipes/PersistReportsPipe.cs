using System.Text;
using AnyKit.Pipelines;
using ChunkIt.Metrics.Host.Serialization;

namespace ChunkIt.Metrics.Host.Plotting.Pipes;

internal sealed class PersistReportsPipe : IPlottingPipe
{
    public async Task Invoke(
        PlottingContext context,
        AsyncPipeline<PlottingContext> next
    )
    {
        var reports = MapReports(context.Reports);

        var json = ReportSerializer.Serialize(reports);
        var jsonPath = context.Output.CreatePathForOutput("data.json");
        await File.WriteAllTextAsync(jsonPath, json, Encoding.UTF8);

        await next(context);
    }

    private static IReadOnlyCollection<Report> MapReports(IReadOnlyCollection<ChunkingReport> reports)
    {
        return reports
            .GroupBySourceFile()
            .Select(entry => new Report
                {
                    File = new Report.FileItem
                    {
                        Name = entry.SourceFile.Name,
                        Size = entry.SourceFile.Size,
                    },
                    Partitioners = entry
                        .Reports
                        .Select(report => new Report.PartitionerItem
                            {
                                Name = report.Input.Partitioner.Name,
                                Metrics = new Report.MetricsItem
                                {
                                    ChunksCount = report.Deduplication.Chunks.Count,
                                    AverageChunkSize = report.Deduplication.AverageChunkSize,
                                    Deduplication = new Report.DeduplicationItem
                                    {
                                        Throughput = report.SavedBytesThroughput.GigabitsPerSecond,
                                        SavedRatio = report.Deduplication.SavedRatio,
                                        VarianceRatio = report.Deduplication.VarianceRatio,
                                        QualityRatio = report.Deduplication.QualityRatio,
                                    },
                                    Performance = new Report.PerformanceItem
                                    {
                                        Throughput = report.Performance.Throughput.GigabitsPerSecond,
                                    },
                                },
                            }
                        )
                        .ToArray(),
                }
            )
            .ToArray();
    }

    private sealed class Report
    {
        public required FileItem File { get; init; }
        public required IReadOnlyCollection<PartitionerItem> Partitioners { get; init; }

        public sealed class FileItem
        {
            public required string Name { get; init; }
            public required long Size { get; init; }
        }

        public sealed class PartitionerItem
        {
            public required string Name { get; init; }
            public required MetricsItem Metrics { get; init; }
        }

        public sealed class MetricsItem
        {
            public required int ChunksCount { get; init; }
            public required int AverageChunkSize { get; init; }

            public required DeduplicationItem Deduplication { get; init; }
            public required PerformanceItem Performance { get; init; }
        }

        public sealed class DeduplicationItem
        {
            public required decimal Throughput { get; init; }

            public required float SavedRatio { get; init; }
            public required float VarianceRatio { get; init; }
            public required float QualityRatio { get; init; }
        }

        public sealed class PerformanceItem
        {
            public required decimal Throughput { get; init; }
        }
    }
}