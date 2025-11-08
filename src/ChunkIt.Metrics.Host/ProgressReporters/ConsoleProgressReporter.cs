using ChunkIt.Common;
using ChunkIt.Common.Abstractions;

namespace ChunkIt.Metrics.Host.ProgressReporters;

internal sealed class ConsoleProgressReporter : IProgressReporter
{
    private Input _lastInput;

    public void Report(Input input, int progress)
    {
        var value = progress switch
        {
            25 => "25%",
            50 => "50%",
            75 => "75%",
            100 => "100%",
            _ => "_",
        };

        if (_lastInput is null)
        {
            _lastInput = input;

            Console.Write($"{input}: {value}");
        }

        if (!input.Equals(_lastInput))
        {
            _lastInput = input;

            Console.Write($"\n{input}: {value}");
        }
        else
        {
            Console.Write(value);
        }
    }
}