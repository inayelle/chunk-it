using ChunkIt.Common.Abstractions;
using ChunkIt.Partitioning.Fixed;
using ChunkIt.Partitioning.Gear;
using ChunkIt.Partitioning.Gear.Table;

namespace ChunkIt.Metrics.Inputs;

internal static class PartitionersProvider
{
    private const int Kilobyte = 1024;

    private const int MinimumChunkSize = 8 * Kilobyte;
    private const int AverageChunkSize = 16 * Kilobyte;
    private const int MaximumChunkSize = 32 * Kilobyte;

    public static IEnumerable<IPartitioner> Enumerate()
    {
        yield return new FixedPartitioner(chunkSize: AverageChunkSize);

        yield return new FastPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3,
            gearTable: GearTable.Predefined(variantIndex: 0, rotations: 0)
        );

        yield return new TwinPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3,
            leftGearTable: GearTable.Predefined(variantIndex: 0, rotations: 0),
            rightGearTable: GearTable.Predefined(variantIndex: 1, rotations: 0)
        );
    }
}