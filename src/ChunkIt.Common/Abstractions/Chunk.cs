namespace ChunkIt.Common.Abstractions;

public readonly struct Chunk
{
    public required long Id { get; init; }
    public required long Offset { get; init; }
    public required int Length { get; init; }
    public required byte[] Hash { get; init; }
}