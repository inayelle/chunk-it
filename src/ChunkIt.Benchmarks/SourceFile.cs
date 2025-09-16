namespace ChunkIt.Benchmarks;

public sealed class SourceFile
{
    public string Path { get; }

    public SourceFile(string path)
    {
        Path = path;
    }

    public override string ToString()
    {
        return System.IO.Path.GetFileName(Path);
    }

    public static implicit operator SourceFile(string path)
    {
        return new SourceFile(path);
    }
}