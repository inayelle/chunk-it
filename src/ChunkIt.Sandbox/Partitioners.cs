using ChunkIt.Common.Abstractions;
using ChunkIt.Partitioners.AsymmetricExtremum;
using ChunkIt.Partitioners.Gear;
using ChunkIt.Partitioners.RapidAsymmetricMaximum;
using ChunkIt.Partitioners.Sequential;

namespace ChunkIt.Sandbox;

internal static class Partitioners
{
    private const int Kilobyte = 1024;

    private const int MinimumChunkSize = 8 * Kilobyte;
    private const int AverageChunkSize = MinimumChunkSize + (MaximumChunkSize - MinimumChunkSize) / 2;
    private const int MaximumChunkSize = 32 * Kilobyte;

    public static readonly IReadOnlyList<IPartitioner> Values = Enumerate().ToArray();

    private static IEnumerable<IPartitioner> Enumerate()
    {
        yield return new GearPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            gearTable: GearTable.Predefined(rotations: 0)
        );

        yield return new FastPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 7,
            gearTable: GearTable.Predefined(rotations: 0)
        );

        yield return new TwinPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 5,
            gearTable: GearTable.Predefined(rotations: 0)
        );

        yield return new TwinPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 5,
            leftGearTable: GearTable.Predefined(rotations: 0),
            rightGearTable: GearTable.Predefined(rotations: 13)
        );

        yield return new RapidAsymmetricMaximumPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize
        );

        yield return new AsymmetricExtremumPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize
        );

        yield return new SequentialPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            mode: SequentialPartitionerMode.Increasing,
            sequenceLength: 5,
            skipLength: 256,
            skipTrigger: 50
        );
    }
}