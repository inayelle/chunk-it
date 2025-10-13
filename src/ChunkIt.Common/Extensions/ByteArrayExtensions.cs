using System.Text;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Common.Extensions;

public static class ByteArrayExtensions
{
    public delegate T GroupByHashSelector<out T>(byte[] hash, IEnumerable<Chunk> chunks);

    public static IEnumerable<(byte[] Hash, IEnumerable<Chunk> Chunks)> GroupByHash(this IEnumerable<Chunk> chunks)
    {
        return chunks.GroupBy(
            static chunk => chunk.Hash,
            static (hash, chunks) => (hash, chunks),
            ByteArrayEqualityComparer.Instance
        );
    }

    public static IEnumerable<T> GroupByHash<T>(this IEnumerable<Chunk> chunks, GroupByHashSelector<T> selector)
    {
        return chunks.GroupBy(
            static chunk => chunk.Hash,
            (hash, groupChunks) => selector(hash, groupChunks),
            ByteArrayEqualityComparer.Instance
        );
    }

    public static IEnumerable<Chunk> DistinctByHash(this IEnumerable<Chunk> chunks)
    {
        return chunks.DistinctBy(
            static chunk => chunk.Hash,
            ByteArrayEqualityComparer.Instance
        );
    }

    public static string ToHexString(this byte[] bytes)
    {
        if (bytes is not { Length: > 0 })
        {
            return String.Empty;
        }

        var sb = new StringBuilder();

        foreach (var value in bytes)
        {
            sb.Append($"{value:x2}");
        }

        return sb.ToString();
    }
}