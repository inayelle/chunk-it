using System.Diagnostics;
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

        foreach (var sourceFilePath in SourceFilePaths.Enumerate())
        {
            foreach (var partitioner in Partitioners.Enumerate())
            {
                var chunkingContext = new ChunkingContext(partitioner, sourceFilePath);

                var stopwatch = Stopwatch.GetTimestamp();
                var chunkingReport = await chunkingPipeline.Invoke(chunkingContext);
                var elapsed = Stopwatch.GetElapsedTime(stopwatch);

                chunkingReports.Add(chunkingReport);

                Console.WriteLine($"Done: {partitioner} {sourceFilePath} in {elapsed.TotalMilliseconds:F2} ms.");
            }
        }

        var plottingContext = new PlottingContext(
            chunkingReports
        );

        await plottingPipeline.Invoke(plottingContext);
    }
}