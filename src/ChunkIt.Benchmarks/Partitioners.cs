using ChunkIt.Common.Abstractions;
using ChunkIt.Partitioning.Fixed;
using ChunkIt.Partitioning.Gear;
using ChunkIt.Partitioning.Gear.Table;

namespace ChunkIt.Benchmarks;

public static class Partitioners
{
    private const int Kilobyte = 1024;

    private const int MinimumChunkSize = 8 * Kilobyte;
    private const int AverageChunkSize = 16 * Kilobyte;
    private const int MaximumChunkSize = 32 * Kilobyte;

    public static IEnumerable<IPartitioner> Enumerate()
    {
        yield return new FixedPartitioner(chunkSize: AverageChunkSize);

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
            gearTable: GearTable.Predefined(rotations: 0)
        );

        // yield return new TwinPartitioner(
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     normalizationLevel: 5,
        //     gearTable: GearTable.Predefined(rotations: 0)
        // );

        yield return new TwinPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 5,
            leftGearTable: GearTable.Predefined(rotations: 0),
            rightGearTable: GearTable.Predefined(rotations: 13)
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

        // yield return new SequentialPartitioner(
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     mode: SequentialPartitionerMode.Increasing,
        //     sequenceLength: 5,
        //     skipLength: 512,
        //     skipTrigger: 50
        // );
        //
        // yield return new SequentialPartitioner(
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     mode: SequentialPartitionerMode.Decreasing,
        //     sequenceLength: 5,
        //     skipLength: 512,
        //     skipTrigger: 50
        // );
    }
}