using ChunkIt.Common.Abstractions;
using ChunkIt.Partitioners.AsymmetricExtremum;
using ChunkIt.Partitioners.Gear;
using ChunkIt.Partitioners.RapidAsymmetricMaximum;
using ChunkIt.Partitioners.Sequential;

namespace ChunkIt.Sandbox;

internal static class Partitioners
{
    private const int Kilobyte = 1024;

    private const int MinimumChunkSize = 4 * Kilobyte;
    private const int AverageChunkSize = 8 * Kilobyte;
    private const int MaximumChunkSize = 16 * Kilobyte;

    public static readonly IReadOnlyList<IPartitioner> Values = Enumerate().ToArray();

    private static IEnumerable<IPartitioner> Enumerate()
    {
        yield return new GearPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            gearTable: new StaticGearTable(rotations: 0)
        );

        yield return new FastPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3,
            gearTable: new StaticGearTable(rotations: 0)
        );

        yield return new TwinPartitioner(
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3,
            leftGearTable: new StaticGearTable(rotations: 0),
            rightGearTable: new StaticGearTable(rotations: 17)
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