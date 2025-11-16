using AnyKit.Pipelines;
using ChunkIt.Metrics.Inputs;

namespace ChunkIt.Metrics.Host.Gathering.Pipes;

internal sealed class ValidateSourceFilesPipe : IGatheringPipe
{
    public Task<IReadOnlyList<ChunkingReport>> Invoke(
        GatheringContext context,
        AsyncPipeline<GatheringContext, IReadOnlyList<ChunkingReport>> next
    )
    {
        var missingSourceFiles = InputsProvider
            .Enumerate()
            .Select(input => input.SourceFile)
            .Distinct()
            .Where(file => !file.Exists)
            .ToArray();

        if (missingSourceFiles.Length > 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.Error.WriteLine("ERR: One or more source files are missing.");

            foreach (var missingSourceFile in missingSourceFiles)
            {
                Console.Error.WriteLine($"     - {missingSourceFile.Path} - not found");
            }

            Console.ResetColor();

            Environment.Exit(1);
        }

        return next(context);
    }
}