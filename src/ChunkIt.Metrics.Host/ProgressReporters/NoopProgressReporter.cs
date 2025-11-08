using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Metrics.Host.ProgressReporters;

internal sealed class NoopProgressReporter : IProgressReporter
{
    public void Report(Input input, int progress)
    {
    }
}