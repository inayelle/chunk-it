using ChunkIt.Common.Abstractions;
using ChunkIt.Metrics.Deduplication.Chunking;
using ChunkIt.Metrics.Deduplication.Plotting;

namespace ChunkIt.Metrics.Deduplication.Controllers;

internal sealed class ChunkingController : IController
{
    private readonly ChunkingPipeline _chunkingPipeline = new();
    private readonly PlottingPipeline _plottingPipeline = new();

    public async Task Run()
    {
        using var _ = SandboxRuntime.Instance;

        await RunCore();
    }

    private async Task RunCore()
    {
        var chunkingReports = new List<ChunkingReport>();

        foreach (var sourceFile in SourceFiles.Values)
        {
            Console.WriteLine($"{sourceFile.Name}:");

            foreach (var partitioner in Partitioners.Values)
            {
                Console.Write($"    - {partitioner}: ");

                try
                {
                    var chunkingReport = await ExecuteChunking(partitioner, sourceFile);

                    chunkingReports.Add(chunkingReport);

                    Console.WriteLine($" completed! Elapsed {chunkingReport.Elapsed.TotalMilliseconds} ms.");
                }
                catch (Exception exception)
                {
                    Console.WriteLine($" failed! {exception}.");
                }
            }

            Console.WriteLine();
        }

        await ExecutePlotting(chunkingReports);
    }

    private Task<ChunkingReport> ExecuteChunking(IPartitioner partitioner, SourceFile sourceFile)
    {
        var chunkingContext = new ChunkingContext(partitioner, sourceFile, DisplayChunkingProgress);

        return _chunkingPipeline.Invoke(chunkingContext);
    }

    private Task ExecutePlotting(IReadOnlyList<ChunkingReport> chunkingReports)
    {
        var plottingContext = new PlottingContext(chunkingReports);

        return _plottingPipeline.Invoke(plottingContext);
    }

    private static void DisplayChunkingProgress(int progress)
    {
        var report = progress switch
        {
            0 => "0%",
            25 => "25%",
            50 => "50%",
            75 => "75%",
            100 => "100%",
            _ => "_",
        };

        Console.Write(report);
    }
}