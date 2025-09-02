using ChunkIt.Abstractions;
using ChunkIt.Partitioners.Entropy;
using ChunkIt.Partitioners.Gear;
using ChunkIt.Partitioners.MeanShift;
using ChunkIt.Partitioners.Ram;
using ChunkIt.Partitioners.Sequential;

namespace ChunkIt.Sandbox;

internal static class Partitioners
{
    private const int Kilobyte = 1024;

    private const int MinimumChunkSize = 1 * Kilobyte;
    private const int AverageChunkSize = 4 * Kilobyte;
    private const int MaximumChunkSize = 8 * Kilobyte;

    public static IEnumerable<IPartitioner> Enumerate()
    {
        yield return new GearPartitioner(
            gearTable: new RandomGearTable(new Random(42)),
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3
        );

        yield return new GearPartitioner(
            gearTable: new StaticGearTable(),
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3
        );

        //
        // yield return new RamPartitioner(
        //     minimumChunkSize: MinimumChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     windowSize: MinimumChunkSize / 2
        // );

        // yield return new SequentialPartitioner(
        //     mode: SequentialPartitionerMode.Increasing,
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     sequenceLength: 5,
        //     skipTrigger: 50,
        //     skipLength: 256
        // );
        //
        // yield return new SequentialPartitioner(
        //     mode: SequentialPartitionerMode.Decreasing,
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     sequenceLength: 5,
        //     skipTrigger: 50,
        //     skipLength: 256
        // );
        //
        // yield return new AdaptiveSequentialPartitioner(
        //     mode: SequentialPartitionerMode.Increasing,
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     sequenceLength: 5,
        //     skipTrigger: 32,
        //     skipLength: 512
        // );
        //
        // yield return new AdaptiveSequentialPartitioner(
        //     mode: SequentialPartitionerMode.Decreasing,
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     sequenceLength: 5,
        //     skipTrigger: 32,
        //     skipLength: 512
        // );

        // yield return new EntropyPartitioner(
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     windowSize: 96,
        //     lowThresholdBits: 1.7,
        //     highThresholdBits: 2.3
        // );
        //
        // yield return new MeanShiftPartitioner(
        //     minimumChunkSize: MinimumChunkSize,
        //     averageChunkSize: AverageChunkSize,
        //     maximumChunkSize: MaximumChunkSize,
        //     window: 48,
        //     kPre: 1.6,
        //     kPost: 1.3,
        //     threshold: 4
        // );
    }
}