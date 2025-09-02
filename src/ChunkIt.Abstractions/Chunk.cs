using System.Text;

namespace ChunkIt.Abstractions;

public sealed class Chunk
{
    public required long Id { get; init; }
    public required long Offset { get; init; }
    public required int Length { get; init; }
    public required byte[] Hash { get; init; }

    public string HashString => ToHexString();

    private string ToHexString()
    {
        var sb = new StringBuilder();

        foreach (var hashByte in Hash)
        {
            sb.Append($"{hashByte:x2}");
        }

        return sb.ToString();
    }
}