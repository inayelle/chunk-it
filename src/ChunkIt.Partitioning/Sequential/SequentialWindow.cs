namespace ChunkIt.Partitioning.Sequential;

internal ref struct SequentialWindow
{
    public int SequenceLength { get; private set; }
    public int ChaosLength { get; private set; }

    public void IncrementSequence()
    {
        SequenceLength += 1;
        ChaosLength = 0;
    }

    public void IncrementChaos()
    {
        SequenceLength = 0;
        ChaosLength += 1;
    }

    public void Reset()
    {
        SequenceLength = 0;
        ChaosLength = 0;
    }
}