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
        var sourceFiles = InputsProvider
            .Enumerate()
            .Select(input => input.SourceFile)
            .Distinct();

        var sourceFileNotFound = false;

        foreach (var sourceFile in sourceFiles)
        {
            var sourceFileExists = File.Exists(sourceFile.Path);

            if (sourceFileExists)
            {
                Console.WriteLine($"INF: source file found at '{sourceFile.Path}'.");
            }
            else
            {
                sourceFileNotFound = true;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"ERR: source file '{sourceFile.Path}' does not exist.");
                Console.ResetColor();
            }
        }

        if (sourceFileNotFound)
        {
            throw new ApplicationException("One or more source files were not found.");
        }

        return next(context);
    }
}