using System.Text;
using ChunkIt.Common.Abstractions;
using ChunkIt.Common.Extensions;

namespace ChunkIt.Sandbox.Extensions;

internal static class SourceFileExtensions
{
    public static string ToPlotTitle(this SourceFile sourceFile)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"RunId: {SandboxRuntime.Instance.RunId:000}");
        sb.Append($"{sourceFile.Name} ({sourceFile.Size.ToHumanReadableSize()})");

        return sb.ToString();
    }

    public static string ToPlotTitle(this SourceFile sourceFile, IPartitioner partitioner)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"RunId: {SandboxRuntime.Instance.RunId:000}");
        sb.AppendLine($"{sourceFile.Name} ({sourceFile.Size.ToHumanReadableSize()})");
        sb.Append(partitioner);

        return sb.ToString();
    }
}