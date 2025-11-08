using ChunkIt.Common.Abstractions;

namespace ChunkIt.Common;

public interface IProgressReporter
{
    void Report(Input input, int progress);
}