using ChunkIt.Common.Extensions;

namespace ChunkIt.Benchmarks;

public sealed class SourceFile : IEquatable<SourceFile>
{
    private const FileOptions DefaultOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;

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

    public FileStream OpenFileStream(int bufferSize = 4096, FileOptions? options = null)
    {
        return new FileStream(
            Path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize,
            options ?? DefaultOptions
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
        return $"{Name} ({Size.ToHumanReadableSize()})";
    }
}