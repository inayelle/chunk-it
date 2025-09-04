using ChunkIt.Common.Extensions;

namespace ChunkIt.Sandbox;

internal sealed class SourceFile : IEquatable<SourceFile>
{
    public string Path { get; }
    public string Name { get; }
    public long Size { get; }

    public SourceFile(string path)
    {
        var fileInfo = new FileInfo(path);

        Path = fileInfo.FullName;
        Name = fileInfo.Name;
        Size = fileInfo.Length;
    }

    public static implicit operator SourceFile(string path)
    {
        return new SourceFile(path);
    }

    public bool Equals(SourceFile other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Path == other.Path && Name == other.Name && Size == other.Size;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is SourceFile other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Path, Name, Size);
    }

    public override string ToString()
    {
        return $"{Path} ({Size.ToHumanReadableSize()})";
    }
}