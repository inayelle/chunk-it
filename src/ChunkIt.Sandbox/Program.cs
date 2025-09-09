using ChunkIt.Sandbox.Chunking;
using ChunkIt.Sandbox.Plotting;

namespace ChunkIt.Sandbox;

internal static class Program
{
    public static async Task Main()
    {
        Console.WriteLine($"=== RUN ID: {SandboxRuntime.Instance.RunId:000} ===");

        await Run();

        Console.WriteLine($"=== RUN ID: {SandboxRuntime.Instance.RunId:000} ===");
    }

    private static async Task Run()
    {
        var chunkingPipeline = new ChunkingPipeline();
        var plottingPipeline = new PlottingPipeline();
        var chunkingReports = new List<ChunkingReport>();

        foreach (var sourceFile in SourceFiles.Values)
        {
            Console.WriteLine($"{sourceFile.Name}:");

            foreach (var partitioner in Partitioners.Values)
            {
                Console.Write($"    - {partitioner}: ");

                var chunkingContext = new ChunkingContext(partitioner, sourceFile, ReportProgress);

                var chunkingReport = await chunkingPipeline.Invoke(chunkingContext);

                chunkingReports.Add(chunkingReport);

                Console.WriteLine($" elapsed: {chunkingReport.Elapsed.TotalMilliseconds} ms.");
            }

            Console.WriteLine();
        }

        var plottingContext = new PlottingContext(
            chunkingReports
        );

        await plottingPipeline.Invoke(plottingContext);
    }

    private static void ReportProgress(int progress)
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