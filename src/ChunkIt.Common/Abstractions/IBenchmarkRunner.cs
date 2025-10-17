namespace ChunkIt.Common.Abstractions;

public interface IBenchmarkRunner<TReport>
{
    Task<Dictionary<Input, TReport>> Run();
}