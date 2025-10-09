using ChunkIt.Sandbox.Chunking;
using ChunkIt.Sandbox.Plotting;

namespace ChunkIt.Sandbox.Controllers;

internal sealed class ChunkingController : IController
{
    public async Task Run()
    {
        using var _ = SandboxRuntime.Instance;

        await RunCore();
    }

    private static async Task RunCore()
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

                if (chunkingReport is not { IsValid: true })
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        $" error: partitioner yielded invalid chunks. " +
                        $"Expected size: {chunkingReport.SourceFile.Size}, " +
                        $"actual size: {chunkingReport.Chunks.Sum(chunk => (long)chunk.Length)}"
                    );
                    Console.ResetColor();
                }

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