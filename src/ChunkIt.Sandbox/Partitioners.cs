using ChunkIt.Abstractions;
using ChunkIt.Partitioners.Gear;

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
            gearTable: new StaticGearTable(),
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3
        );

        yield return new CentricGearPartitioner(
            gearTable: new StaticGearTable(),
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3
        );

        yield return new SlidingGearPartitioner(
            gearTable: new StaticGearTable(),
            minimumChunkSize: MinimumChunkSize,
            averageChunkSize: AverageChunkSize,
            maximumChunkSize: MaximumChunkSize,
            normalizationLevel: 3
        );
    }
}