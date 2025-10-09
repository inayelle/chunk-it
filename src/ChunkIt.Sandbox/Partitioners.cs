using ChunkIt.Common.Abstractions;
using ChunkIt.Partitioning.AsymmetricExtremum;
using ChunkIt.Partitioning.Fixed;
using ChunkIt.Partitioning.Gear;
using ChunkIt.Partitioning.Rabin;
using ChunkIt.Partitioning.RapidAsymmetricMaximum;
using ChunkIt.Partitioning.Sequential;

namespace ChunkIt.Sandbox;

internal static class Partitioners
{
    private const int Kilobyte = 1024;

    private const int MinimumChunkSize = 8 * Kilobyte;
    private const int AverageChunkSize = 16 * Kilobyte;
    private const int MaximumChunkSize = 32 * Kilobyte;

    public static readonly IReadOnlyList<IPartitioner> Values = Enumerate().ToArray();

    private static IEnumerable<IPartitioner> Enumerate()
    {
        // yield return new FixedPartitioner(chunkSize: AverageChunkSize);
        //
        // yield return new RabinPartitioner(
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize
        // );
        //
        // yield return new GearPartitioner(
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     gearTable: GearTable.Predefined(rotations: 0)
        // );

        yield return new FastPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3,
            gearTable: GearTable.Predefined(rotations: 0, tableIndex: 0)
        );

        yield return new TwinPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3,
            gearTable: GearTable.Predefined(rotations: 0, tableIndex: 0)
        );

        yield return new TwinPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3,
            leftGearTable: GearTable.Predefined(rotations: 0, tableIndex: 0),
            rightGearTable: GearTable.Predefined(rotations: 0, tableIndex: 1)
        );

        // yield return new RapidAsymmetricMaximumPartitioner(
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize
        // );
        //
        // yield return new AsymmetricExtremumPartitioner(
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize
        // );
        //
        // yield return new SequentialPartitioner(
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     mode: SequentialPartitionerMode.Increasing,
        //     sequenceLength: 5,
        //     skipLength: 512,
        //     skipTrigger: 50
        // );
    }
}