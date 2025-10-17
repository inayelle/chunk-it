using ChunkIt.Common.Abstractions;

namespace ChunkIt.Metrics.Inputs;

public static class InputsProvider
{
    public static IEnumerable<Input> Enumerate()
    {
        var index = 0;

        foreach (var sourceFile in SourceFilesProvider.Enumerate())
        {
            foreach (var partitioner in PartitionersProvider.Enumerate())
            {
                yield return new Input(sourceFile, partitioner, index++);
            }
        }
    }
}