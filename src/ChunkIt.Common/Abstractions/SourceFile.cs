using ChunkIt.Common.Extensions;

namespace ChunkIt.Common.Abstractions;

public sealed class SourceFile : IEquatable<SourceFile>
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

    public FileStream OpenFileStream(
        int bufferSize = 4096,
        FileShare share = FileShare.None,
        FileOptions options = FileOptions.SequentialScan
    )
    {
        return new FileStream(
            Path,
            FileMode.Open,
            FileAccess.Read,
            share,
            bufferSize,
            options
        );
    }

    public string GenerateVersionedPath(string version)
    {
        var fileName = System.IO.Path.GetFileNameWithoutExtension(Path);
        var extension = System.IO.Path.GetExtension(Path);
        var filePath = System.IO.Path.GetDirectoryName(Path)!;

        return System.IO.Path.Combine(
            filePath,
            $"{fileName}-{version}{extension}"
        );
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