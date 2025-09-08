using ChunkIt.Common.Abstractions;
using ChunkIt.Partitioners.Gear;
using ChunkIt.Partitioners.Rabin;

namespace ChunkIt.Sandbox;

internal static class Partitioners
{
    private const int Kilobyte = 1024;

    private const int MinimumChunkSize = 4 * Kilobyte;
    private const int AverageChunkSize = 16 * Kilobyte;
    private const int MaximumChunkSize = 32 * Kilobyte;

    public static readonly IReadOnlyList<IPartitioner> Values =
    [
        new RabinPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize
        ),

        new GearPartitioner(
            gearTable: new StaticGearTable(),
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3
        ),

        new TwinGearPartitioner(
            gearTable: new StaticGearTable(rotations: 0),
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3
        ),

        new TwinGearPartitioner(
            leftGearTable: new StaticGearTable(rotations: 0),
            rightGearTable: new StaticGearTable(rotations: 17),
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3
        ),

        // new SequentialPartitioner(
        //     mode: SequentialPartitionerMode.Increasing,
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     sequenceLength: 5,
        //     skipTrigger: 50,
        //     skipLength: 256
        // ),
        //
        // new SequentialPartitioner(
        //     mode: SequentialPartitionerMode.Decreasing,
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     sequenceLength: 5,
        //     skipTrigger: 50,
        //     skipLength: 256
        // ),
        //
        // new AdaptiveSequentialPartitioner(
        //     mode: SequentialPartitionerMode.Increasing,
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     sequenceLength: 5,
        //     skipTrigger: 50,
        //     skipLength: 256
        // ),

        // new SlidingGearPartitioner(
        //     gearTable: new StaticGearTable(),
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     normalizationLevel: 3
        // ),
        //
        // new RamPartitioner(
        //     minimumChunkSize: MinimumChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     windowSize: AverageChunkSize
        // ),
        //
        // new SequentialPartitioner(
        //     mode: SequentialPartitionerMode.Increasing,
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     sequenceLength: 5,
        //     skipLength: 50,
        //     skipTrigger: 256
        // ),
        //
        // new SequentialPartitioner(
        //     mode: SequentialPartitionerMode.Decreasing,
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     sequenceLength: 5,
        //     skipLength: 50,
        //     skipTrigger: 256
        // ),
        //
        // new AdaptiveSequentialPartitioner(
        //     mode: SequentialPartitionerMode.Increasing,
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     sequenceLength: 5,
        //     skipLength: 50,
        //     skipTrigger: 256
        // ),
        //
        // new AdaptiveSequentialPartitioner(
        //     mode: SequentialPartitionerMode.Decreasing,
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     sequenceLength: 5,
        //     skipLength: 50,
        //     skipTrigger: 256
        // ),
        //
        // new MeanShiftPartitioner(
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize
        // ),
        //
        // new EntropyPartitioner(
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     windowSize: 64,
        //     lowThresholdBits: 1.25,
        //     highThresholdBits: 1.85
        // ),
    ];
}