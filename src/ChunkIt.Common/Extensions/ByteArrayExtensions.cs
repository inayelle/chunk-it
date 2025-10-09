using System.Text;

namespace ChunkIt.Common.Extensions;

public static class ByteArrayExtensions
{
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