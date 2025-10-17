namespace ChunkIt.Common.Abstractions;

public sealed class Input : IEquatable<Input>, IComparable<Input>
{
    public SourceFile SourceFile { get; }
    public IPartitioner Partitioner { get; }

    public int Index { get; }

    public Input(SourceFile sourceFile, IPartitioner partitioner, int index)
    {
        SourceFile = sourceFile;
        Partitioner = partitioner;
        Index = index;
    }

    public bool Equals(Input other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Index == other.Index && SourceFile.Equals(other.SourceFile) && Partitioner.Equals(other.Partitioner);
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Input other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Index, SourceFile, Partitioner);
    }

    public int CompareTo(Input other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        return Index.CompareTo(other.Index);
    }

    public override string ToString()
    {
        return $$"""Input {Id: {{Index}} SourceFile: {{SourceFile}} Partitioner: {{Partitioner}}}""";
    }
}